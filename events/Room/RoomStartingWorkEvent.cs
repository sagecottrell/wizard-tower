/**
Generated from ./events/Room/RoomStartedWorkEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomStartingWorkEvent(TowerState towerState, RoomState roomState) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
}


public static class RoomStartedWorkEventExtensions {
    public static RoomStartedWorkEvent Into(this RoomStartingWorkEvent old) {
        return new(towerState: old.TowerState, roomState: old.RoomState) { Source = old, };
    }

    public static RoomStartingWorkEvent RoomStartingWorkEvent(this IEvent ev, TowerState towerState, RoomState roomState)
    {
        return new(towerState, roomState) { Source = ev };
    }
}