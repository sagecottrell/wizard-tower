using wizardtower.events.handlers;
using wizardtower.events.Room;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void BuyRoom(RoomConstructingEvent @event)
    {
        var tower = @event.TowerState;
        if (RoomEvents.OnRoomConstructing(@event).IsAllowed)
        {
            RemoveFromWallet(new(tower, @event.Room.Definition.CostToBuildPerUnit) { Source = @event });
            tower.AddRoom(@event.Room);
            RoomEvents.OnRoomConstructed(new(tower, @event.Room) { Source = @event.Source });
        }
    }
}
