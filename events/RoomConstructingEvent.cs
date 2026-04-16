using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class RoomConstructingEvent(TowerState tower, RoomState room) : BaseEvent, IAllowableEvent, ITowerEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public RoomState Room { get; } = room;
}
