/**
Generated from ./events/Room/RoomAssignedOutputEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomAssigningOutputEvent(TowerState tower, RoomState room, RoomState targetRoom, RoomStateWorkerPath path) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public RoomState RoomState { get; set; } = room;
    public RoomState TargetRoom { get; set; } = targetRoom;
    public RoomStateWorkerPath Path { get; set; } = path;
}


public static class RoomAssignedOutputEventExtensions {
    public static RoomAssignedOutputEvent Into(this RoomAssigningOutputEvent old) {
        return new(tower: old.TowerState, room: old.RoomState, targetRoom: old.TargetRoom, path: old.Path) { Source = old, Input = old.Input, };
    }
}