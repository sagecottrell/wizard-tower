using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class RoomSelectingEvent(TowerState tower, RoomState room) : BaseEvent, ITowerEvent, IAllowableEvent, IRoomEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public RoomState Room { get; set; } = room;
}
