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
        if (RoomEvents.OnConstructing(ev).IsAllowed)
        {
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
    }

    public static void Destroy(RoomDestroyingEvent ev)
    {
        if (RoomEvents.OnDestroying(ev).IsAllowed)
        {
            ev.TowerState.RemoveRoom(ev.Room);
            RoomEvents.OnDestroyed(new(ev.TowerState, ev.Room) { Source = ev.Source });
        }
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
        if (ev.Output is not null)
        {
            if (ev.RoomState.Definition.ResourceConversion?.ToTowerWallet == true)
                TowerActions.AddToWallet(new(ev.TowerState, ev.Output) { Source = ev });
            else
                ev.RoomState.StoredItems += ev.Output;
            StopWork(new(ev.TowerState, ev.RoomState) { Source = ev });
            conv.TimesProducedToday++;
            RoomEvents.OnProducedResources(new(ev.TowerState, ev.RoomState) { Source = ev, Output = ev.Output });
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
}
