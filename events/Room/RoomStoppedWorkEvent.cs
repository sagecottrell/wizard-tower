using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomStoppedWorkEvent(TowerState towerState, RoomState roomState) : BaseEvent, ITowerEvent, IRoomEvent
{
    public TowerState TowerState { get; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
}
