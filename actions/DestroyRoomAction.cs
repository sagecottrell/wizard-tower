using wizardtower.events;
using wizardtower.events.handlers;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void DestroyRoom(RoomDestroyingEvent @event)
    {
        if (RoomEvents.OnRoomDestroying(@event).IsAllowed)
        {
            @event.TowerState.RemoveRoom(@event.Room);
            RoomEvents.OnRoomDestroyed(new(@event.TowerState, @event.Room) { Source = @event.Source });
        }
    }
}
