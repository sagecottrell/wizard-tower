using wizardtower.events;
using wizardtower.state;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void ExtendFloor(TowerState state, FloorExtendingEvent @event)
    {
        var cost = @event.Floor.Definition.CostToBuildPerUnit * @event.ExtensionAmount;
        state.ExtendFloor(@event.Floor, @event.ExtendedLeft, @event.ExtendedRight);
        if (GlobalSignals.TowerResourceChanging(new(state, cost, @event)).IsAllowed)
        {
            state.Wallet.Subtracted(cost);
            GlobalSignals.TowerResourceChanged(new(state, cost, @event));
        }
    }
}
