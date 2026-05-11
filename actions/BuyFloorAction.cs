using wizardtower.events.Floor;
using wizardtower.events.handlers;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void BuyFloor(FloorConstructingEvent @event)
    {
        var tower = @event.TowerState;
        if (FloorEvents.OnFloorConstructing(@event).IsAllowed)
        {
            RemoveFromWallet(new(tower, @event.Floor.Definition.CostToBuildPerUnit * @event.Floor.Width) { Source = @event });
            tower.OnAddFloor(@event.Floor);
            FloorEvents.OnFloorConstructed(new(tower, @event.Floor) { Source = @event.Source });
        }
    }
}
