using wizardtower.events;
using wizardtower.events.handlers;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void ExtendFloor(FloorExtendingEvent @event)
    {
        var tower = @event.TowerState;
        if (FloorEvents.OnFloorExtending(@event).IsAllowed)
        {
            RemoveFromWallet(new(tower, @event.Floor.Definition.CostToBuildPerUnit * @event.ExtensionAmount) { Source = @event });
            tower.ExtendFloor(@event.Floor, @event.ExtendedLeft, @event.ExtendedRight);
            FloorEvents.OnFloorExtended(new(tower, @event.Floor, @event.ExtendedLeft, @event.ExtendedRight) { Source = @event.Source });
        }
    }
}
