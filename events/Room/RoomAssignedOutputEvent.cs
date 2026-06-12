using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomAssignedOutputEvent(TowerState tower, RoomState room, RoomState targetRoom, RoomStateWorkerPath path) : BaseEvent, ITowerEvent, IRoomEvent
{
    public TowerState TowerState { get; } = tower;
    public RoomState RoomState { get; } = room;
    public RoomState TargetRoom { get; } = targetRoom;
    public RoomStateWorkerPath Path { get; } = path;
}
