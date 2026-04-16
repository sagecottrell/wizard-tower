using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class RoomDestroyingEvent(TowerState towerState, RoomState room) : BaseEvent, ITowerEvent, IDebug, IAllowableEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public RoomState Room { get; } = room;
}
