using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomAssigningOutputEvent(TowerState tower, RoomState room, RoomState targetRoom, RoomStateWorkerPath path) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public RoomState RoomState { get; set; } = room;
    public RoomState TargetRoom { get; set; } = targetRoom;
    public RoomStateWorkerPath Path { get; set; } = path;
}
