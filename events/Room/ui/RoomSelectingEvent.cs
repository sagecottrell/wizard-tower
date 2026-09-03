/**
Generated from ./events/Room/ui/RoomSelectedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

public partial class RoomSelectingEvent(TowerState tower, RoomState room) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public RoomState RoomState { get; set; } = room;
}


public static class RoomSelectedEventExtensions {
    public static RoomSelectedEvent Into(this RoomSelectingEvent old) {
        return new(tower: old.TowerState, room: old.RoomState) { Source = old, };
    }

    public static RoomSelectingEvent RoomSelectingEvent(this IEvent ev, TowerState tower, RoomState room)
    {
        return new(tower, room) { Source = ev };
    }
}