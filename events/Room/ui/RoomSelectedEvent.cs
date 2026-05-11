using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

public partial class RoomSelectedEvent(TowerState tower, RoomState room) : BaseEvent, ITowerEvent, IRoomEvent, IDebug
{
    public TowerState TowerState { get; } = tower;
    public RoomState RoomState { get; set; } = room;
}
