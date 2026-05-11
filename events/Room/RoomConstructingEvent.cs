using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room;

public partial class RoomConstructingEvent(TowerState tower, RoomState room) : BaseEvent, IDeniableEvent, ITowerEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public RoomState Room { get; } = room;
}
