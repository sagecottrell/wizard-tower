/**
Generated from ./events/Room/RoomConstructedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public partial class RoomConstructingEvent(TowerState tower, RoomState room) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public RoomState Room { get; set; } = room;
}


public static class RoomConstructedEventExtensions {
    public static RoomConstructedEvent Into(this RoomConstructingEvent old) {
        return new(tower: old.TowerState, room: old.Room) { Source = old, };
    }

    public static RoomConstructingEvent RoomConstructingEvent(this IEvent ev, TowerState tower, RoomState room)
    {
        return new(tower, room) { Source = ev };
    }
}