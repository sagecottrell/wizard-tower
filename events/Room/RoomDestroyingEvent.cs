/**
Generated from ./events/Room/RoomDestroyedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public partial class RoomDestroyingEvent(TowerState towerState, RoomState room) : BaseEvent, IDeniableEvent, ITowerEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public RoomState Room { get; set; } = room;
}


public static class RoomDestroyedEventExtensions {
    public static RoomDestroyedEvent Into(this RoomDestroyingEvent old) {
        return new(towerState: old.TowerState, room: old.Room) { Source = old, };
    }

    public static RoomDestroyingEvent RoomDestroyingEvent(this IEvent ev, TowerState towerState, RoomState room)
    {
        return new(towerState, room) { Source = ev };
    }
}