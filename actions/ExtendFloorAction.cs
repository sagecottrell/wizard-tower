using wizardtower.events;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void ExtendFloor(FloorExtendingEvent @event)
    {
        var tower = @event.TowerState;
        if (GlobalSignals.FloorExtending(@event).IsAllowed)
        {
            RemoveFromWallet(tower, @event.Floor.Definition.CostToBuildPerUnit * @event.ExtensionAmount, @event);
            tower.ExtendFloor(@event.Floor, @event.ExtendedLeft, @event.ExtendedRight);
            GlobalSignals.FloorExtended(new(tower, @event.Floor, @event.ExtendedLeft, @event.ExtendedRight));
        }
    }
}
