using wizardtower.events;
using wizardtower.state;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void BuyRoom(TowerState state, RoomConstructingEvent @event)
    {
        if (GlobalSignals.RoomConstructing(@event).IsAllowed)
        {
            RemoveFromWallet(state, @event.Room.Definition.CostToBuildPerUnit, @event);
            state.OnAddRoom(@event.Room);
            GlobalSignals.RoomConstructed(new(state, @event.Room));
        }
    }
}
