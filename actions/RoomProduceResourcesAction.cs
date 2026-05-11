using wizardtower.events.handlers;
using wizardtower.events.Room;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void RoomProduceResources(RoomProducingResourcesEvent @event)
    {
        if (!RoomEvents.OnRoomProducingResources(@event).IsAllowed)
            return;
        if (@event.ResetProductionProgress)
            @event.RoomConvertResourcesState.ProductionProgress = 0;
        @event.Output ??= @event.RoomConvertResourcesDefinition.Recipe.Output?.PickWeightedRandom(@event.TowerState.RandomNumberGenerator, x => x.Weight)?.Output;
        if (@event.Output is not null)
        {
            if (@event.RoomConvertResourcesDefinition.ToTowerWallet)
                AddToWallet(new(@event.TowerState, @event.Output) { Source = @event });
            else
            {
                @event.RoomState.StoredItems += @event.Output;
            }
            RoomEvents.OnRoomProducedResources(new(@event.TowerState, @event.RoomState, @event.RoomConvertResourcesDefinition, @event.RoomConvertResourcesState) { Source = @event });
        }
    }
}
