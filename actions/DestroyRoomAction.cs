using wizardtower.events;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void DestroyRoom(RoomDestroyingEvent @event)
    {
        if (GlobalSignals.RoomDestroying(@event).IsAllowed)
        {
            @event.TowerState.RemoveRoom(@event.Room);
            GlobalSignals.RoomDestroyed(new(@event.TowerState, @event.Room, @event));
        }
    }
}
