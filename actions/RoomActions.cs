using wizardtower.events.handlers;
using wizardtower.events.Room;
using wizardtower.state;

namespace wizardtower.actions;

public static class RoomActions
{
    public static void Construct(RoomConstructingEvent ev)
    {
        var tower = ev.TowerState;
        if (RoomEvents.OnConstructing(ev).IsAllowed)
        {
            TowerActions.RemoveFromWallet(new(tower, ev.Room.Definition.CostToBuildPerUnit) { Source = ev });
            tower.AddRoom(ev.Room);
            RoomEvents.OnConstructed(new(tower, ev.Room) { Source = ev.Source });
        }
    }

    public static void Destroy(RoomDestroyingEvent ev)
    {
        if (RoomEvents.OnDestroying(ev).IsAllowed)
        {
            ev.TowerState.RemoveRoom(ev.Room);
            RoomEvents.OnDestroyed(new(ev.TowerState, ev.Room) { Source = ev.Source });
        }
    }

    public static void ProduceResources(RoomProducingResourcesEvent ev)
    {
        if (!RoomEvents.OnProducingResources(ev).IsAllowed)
            return;
        if (ev.ResetProductionProgress)
            ev.RoomConvertResourcesState.ProductionProgress = 0;
        ev.Output ??= ev.RoomConvertResourcesDefinition.Recipe.Output?.PickWeightedRandom(ev.TowerState.RandomNumberGenerator, x => x.Weight)?.Output;
        if (ev.Output is not null)
        {
            if (ev.RoomConvertResourcesDefinition.ToTowerWallet)
                TowerActions.AddToWallet(new(ev.TowerState, ev.Output) { Source = ev });
            else
                ev.RoomState.StoredItems += ev.Output;
            ev.RoomConvertResourcesState.CurrentlyWorking = false;
            RoomEvents.OnProducedResources(new(ev.TowerState, ev.RoomState, ev.RoomConvertResourcesDefinition, ev.RoomConvertResourcesState) { Source = ev });
        }
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

    public static void SpawnWorkerWithPayload(RoomDispatchingWorkerEvent ev)
    {
        if (!RoomEvents.OnRoomDispatchingWorker(ev).IsAllowed)
            return;
        var worker = new WorkerState()
        {
            DestinationRoomId = ev.TargetRoom.Id,
            FloorPosition = ev.RoomState.FloorPosition,
            Elevation = ev.RoomState.Elevation,
            PayloadAmount = ev.Amount,
            PayloadKind = ev.Item,
            SourceRoomId = ev.RoomState.Id,
            WorkerDefinition = ev.WorkerDefinition,
        };
        WorkerActions.Dispatch(new(ev.TowerState, worker) { Source = ev });
    }

    public static void StartWork(RoomStartingWorkEvent ev)
    {
        if (!RoomEvents.OnRoomStartingWork(ev).IsAllowed)
            return;
        ev.RoomConvertResourcesState.CurrentlyWorking = true;
        RoomEvents.OnRoomStartedWork(new(ev.TowerState, ev.RoomState, ev.RoomConvertResourcesDefinition, ev.RoomConvertResourcesState) { Source = ev });
    }
}
