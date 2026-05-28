using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomStartingWorkEvent(TowerState towerState, RoomState roomState) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
}
