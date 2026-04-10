using wizardtower.events;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void BuyRoom(RoomConstructingEvent @event)
    {
        var tower = @event.TowerState;
        if (GlobalSignals.RoomConstructing(@event).IsAllowed)
        {
            RemoveFromWallet(tower, @event.Room.Definition.CostToBuildPerUnit, @event);
            tower.AddRoom(@event.Room);
            GlobalSignals.RoomConstructed(new(tower, @event.Room));
        }
    }
}
