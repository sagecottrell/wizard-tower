/**
Generated from ./events/Room/ui/RoomDeselectedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

public partial class RoomDeselectingEvent(TowerState tower, RoomState room) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public RoomState RoomState { get; set; } = room;
}


public static class RoomDeselectedEventExtensions {
    public static RoomDeselectedEvent Into(this RoomDeselectingEvent old) {
        return new(tower: old.TowerState, room: old.RoomState) { Source = old, };
    }
}