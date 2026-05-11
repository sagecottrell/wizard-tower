using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

public partial class RoomDeselectingEvent(TowerState tower, RoomState room) : BaseEvent, ITowerEvent, IDeniableEvent, IRoomEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public RoomState RoomState { get; set; } = room;
}
