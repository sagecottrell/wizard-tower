using wizardtower.events;
using wizardtower.state;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void ExtendFloor(TowerState state, FloorExtendingEvent @event)
    {
        if (GlobalSignals.FloorExtending(@event).IsAllowed)
        {
            RemoveFromWallet(state, @event.Floor.Definition.CostToBuildPerUnit * @event.ExtensionAmount, @event);
            state.ExtendFloor(@event.Floor, @event.ExtendedLeft, @event.ExtendedRight);
            GlobalSignals.FloorExtended(new(state, @event.Floor, @event.ExtendedLeft, @event.ExtendedRight));
        }
    }
}
