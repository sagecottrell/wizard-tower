using wizardtower.events.handlers;
using wizardtower.events.Room;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.actions;

public static class RoomActions
{
    public static void Construct(RoomConstructingEvent ev)
    {
        var tower = ev.TowerState;
        if (!RoomEvents.OnConstructing(ev).IsAllowed)
            return;
        TowerActions.RemoveFromWallet(new(tower, ev.Room.Definition.CostToBuildPerUnit) { Source = ev });
        tower.AddRoom(ev.Room);
        if (ev.Room.Definition.ResourceConversion is not null)
        {
            ev.Room.ConvertResourcesState = new();
            if (ev.Room.ConvertResourcesState.SelectedRecipe is null && ev.Room.Definition.ResourceConversion.Recipes.Recipes.Count == 1)
                ev.Room.ConvertResourcesState.SelectedRecipe = ev.Room.Definition.ResourceConversion.Recipes.Recipes[0];
        }
        RoomEvents.OnConstructed(new(tower, ev.Room) { Source = ev.Source });
    }

    public static void Destroy(RoomDestroyingEvent ev)
    {
        if (!RoomEvents.OnDestroying(ev).IsAllowed)
            return;
        ev.TowerState.RemoveRoom(ev.Room);
        RoomEvents.OnDestroyed(new(ev.TowerState, ev.Room) { Source = ev.Source });
    }

    public static void ProductionProgress(RoomProcessingIncreasingEvent ev)
    {
        if (!RoomEvents.OnProcessingIncreasing(ev).IsAllowed)
            return;
        ev.State.ProductionProgress += ev.AmountIncreased;
        RoomEvents.OnProcessingIncreased(new(ev.RoomState, ev.State) { Source = ev });
    }

    public static void ProduceResources(RoomProducingResourcesEvent ev)
    {
        if (!RoomEvents.OnProducingResources(ev).IsAllowed)
            return;
        var conv = ev.RoomState.ConvertResourcesState;
        if (conv is null)
            return;
        if (conv.SelectedRecipe is null)
            return;
        if (ev.ResetProductionProgress)
            conv.ProductionProgress = 0;
        ev.Output ??= conv.SelectedRecipe.Output?.PickWeightedRandom(ev.TowerState.RandomNumberGenerator, x => x.Weight)?.Output;
        if (ev.Output is null)
            return;

        if (ev.RoomState.Definition.ResourceConversion?.ToTowerWallet == true)
            TowerActions.AddToWallet(new(ev.TowerState, ev.Output) { Source = ev });
        else
            ev.RoomState.StoredItems += ev.Output;
        StopWork(new(ev.TowerState, ev.RoomState) { Source = ev });
        conv.TimesProducedToday++;
        RoomEvents.OnProducedResources(new(ev.TowerState, ev.RoomState) { Source = ev, Output = ev.Output });
    }

    public static void ConsumeResources(RoomConsumingResourcesEvent ev)
    {
        if (!RoomEvents.OnConsumingResources(ev).IsAllowed)
            return;
        ev.RoomState.StoredItems.Subtracted(ev.Amount);
        RoomEvents.OnConsumedResources(new(ev.TowerState, ev.RoomState, ev.Amount) { Source = ev });
    }

    public static void ReceiveResources(RoomReceivingResourcesEvent ev)
    {
        if (!RoomEvents.OnRoomReceivingResources(ev).IsAllowed)
            return;
        ev.RoomState.StoredItems.Added(ev.Resources);
        RoomEvents.OnRoomReceivedResources(new(ev.TowerState, ev.RoomState, ev.Resources) { Source = ev });
    }

    public static void SpawnWorkerWithPayload(TowerState towerState, RoomState roomState, RoomState targetRoom, ItemDefinition item, uint amount, WorkerDefinition def)
    {
        var worker = new WorkerState()
        {
            DestinationRoomId = targetRoom.Id,
            FloorPosition = roomState.FloorPosition,
            Elevation = roomState.Elevation,
            PayloadAmount = amount,
            PayloadKind = item,
            SourceRoomId = roomState.Id,
            WorkerDefinition = def,
        };
        WorkerActions.Dispatch(new(towerState, worker));
    }

    public static void StartWork(RoomStartingWorkEvent ev)
    {
        if (!RoomEvents.OnStartingWork(ev).IsAllowed)
            return;
        if (ev.RoomState.ConvertResourcesState is null)
            return;
        ev.RoomState.ConvertResourcesState.CurrentlyWorking = true;
        RoomEvents.OnStartedWork(new(ev.TowerState, ev.RoomState) { Source = ev });
    }

    public static void StopWork(RoomStoppingWorkEvent ev)
    {
        if (!RoomEvents.OnStoppingWork(ev).IsAllowed)
            return;
        if (ev.RoomState.ConvertResourcesState is null)
            return;
        ev.RoomState.ConvertResourcesState.CurrentlyWorking = false;
        RoomEvents.OnStoppedWork(new(ev.TowerState, ev.RoomState) { Source = ev });
    }

    /// <summary>
    /// Find the nearest rooms that have unallocated supplies for this room
    /// </summary>
    public static void AutoAssignInputs(TowerState tower, RoomState room)
    {
        //if (room.InputRate is not { } inputRate || tower.ScheduledInputRate(room) is not { } onTheWay)
        //    return;

        //var unfulfilledInputs = inputRate - onTheWay;
        //unfulfilledInputs.RemovedZeroes();
        //if (unfulfilledInputs.Count == 0)
        //    return;

        //var possibleRooms = new List<RoomState>();
        //foreach (var otherRoom in tower.IterRoomsFrom(room.Elevation))
        //{
        //    if (otherRoom == room)
        //        continue;
        //    // if this room does not have something we want, we can skip it.
        //    // meaning: there are no possible outputs from the selected recipe, or it's not a warehouse
        //    if (otherRoom.ConvertResourcesState?.SelectedRecipe?.PossibleOutputs.Any(unfulfilledInputs.ContainsKey) != true
        //        && (otherRoom.Warehouse is null || !unfulfilledInputs.ContainsKey(otherRoom.Warehouse)))
        //        continue;
        //    possibleRooms.Add(otherRoom);
        //}

        //var orderedRooms = possibleRooms
        //    .Select(r => new { room = r, dist = TowerPathfind.Pathfind(tower, room, r, 10) })
        //    .Where(i => i.dist != null)
        //    .OrderBy(r => r.room.Warehouse is null || !unfulfilledInputs.ContainsKey(r.room.Warehouse) ? r.dist!.Count : r.dist!.Count / 2);
        //foreach (var otherRoom in orderedRooms)
        //{
        //    var outputRate = otherRoom.room.OutputRate;
        //    if (otherRoom.room.ConvertResourcesState?.SelectedRecipe?.PossibleOutputs is { } outputs && outputRate is not null)
        //    {
        //        foreach (var o in outputs)
        //        {
        //            if (unfulfilledInputs.TryGetValue(o, out var amt) && amt > 0)
        //            {
        //                unfulfilledInputs[o] -= Math.Min(outputRate[o], amt);
        //                var path = otherRoom.dist.GetPath();
        //                otherRoom.room.WorkerPaths.Add(new() { ItemDefinition=o, TargetRoomId=room.Id, ToWhichFloors = [..path.Select(x => x.Elevation)] });
        //            }
        //        }
        //    }
        //}
    }

    /// <summary>
    /// Find the nearest rooms that require all of this room's unallocated supplies
    /// </summary>
    public static void AutoAssignOutputs(RoomState room)
    {

    }

    /// <summary>
    /// Find the nearest rooms that have unallocated workers that are required for this room
    /// </summary>
    /// <param name="room"></param>
    public static void AutoAssignWorkersToSelf(RoomState room)
    {

    }

    /// <summary>
    /// Find the nearest rooms that require the workers provided by this room
    /// </summary>
    /// <param name="room"></param>
    public static void AutoAssignWorkersToOthers(RoomState room)
    {

    }

    /// <summary>
    /// Remove all inputs from this room, effectively unallocating supplies in the supplier rooms
    /// </summary>
    public static void InvalidateInputs(RoomState room)
    {

    }

    /// <summary>
    /// Remove all outgoing resource allocations from this room
    /// </summary>
    public static void InvalidateOutputs(RoomState room)
    {

    }

    /// <summary>
    /// Unallocate workers assigned to other rooms
    /// </summary>
    /// <param name="room"></param>
    public static void InvalidateRecallWorkers(RoomState room)
    {

    }

    /// <summary>
    /// Unallocate workers assigned to this room
    /// </summary>
    /// <param name="room"></param>
    public static void InvalidateFireWorkers(RoomState room)
    {

    }

    public static void AssignOutput(RoomAssigningOutputEvent ev)
    {
        if (!RoomEvents.OnAssigningOutput(ev).IsAllowed) return;

        ev.RoomState.WorkerPaths.Add(ev.Path);
        if (ev.Path.TransportsToTake is null)
        {
            ev.Path.TransportsToTake = TowerPathfind.Pathfind(ev.TowerState, ev.RoomState, ev.TargetRoom, 4);
        }
        RoomEvents.OnAssignedOutput(new(ev.TowerState, ev.RoomState, ev.TargetRoom, ev.Path) { Source = ev });
    }

    public static void RemoveOutput(RoomState room, RoomStateWorkerPath path)
    {
        if (!room.WorkerPaths.Remove(path))
            return;
    }

    public static void AssignWorker(RoomState room, WorkerDefinition wdef, uint amount)
    {
        if (amount == 0)
            return;
        room.StoredWorkers[wdef] = room.StoredWorkers.GetOrDefault(wdef) + amount;
    }

    public static void UnassignWorker(RoomState room, WorkerDefinition wdef, uint amount)
    {
        if (amount == 0)
            return;
        if (room.StoredWorkers.GetOrDefault(wdef) < amount)
            return;
        room.StoredWorkers[wdef] -= amount;
    }
}
