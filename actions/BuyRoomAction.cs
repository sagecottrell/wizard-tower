using wizardtower.events;
using wizardtower.state;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void BuyRoom(TowerState state, RoomConstructedEvent @event)
    {
        var cost = @event.Room.Definition.CostToBuildPerUnit;
        state.OnAddRoom(@event.Room);
        if (GlobalSignals.TowerResourceChanging(new(state, cost, @event)).IsAllowed)
        {
            state.Wallet.Subtracted(cost);
            GlobalSignals.TowerResourceChanged(new(state, cost, @event));
        }
    }
}
