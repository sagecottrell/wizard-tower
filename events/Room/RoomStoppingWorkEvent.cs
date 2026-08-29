/**
Generated from ./events/Room/RoomStoppedWorkEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomStoppingWorkEvent(TowerState towerState, RoomState roomState) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
}


public static class RoomStoppedWorkEventExtensions {
    public static RoomStoppedWorkEvent Into(this RoomStoppingWorkEvent old) {
        return new(towerState: old.TowerState, roomState: old.RoomState) { Source = old, Input = old.Input, };
    }
}