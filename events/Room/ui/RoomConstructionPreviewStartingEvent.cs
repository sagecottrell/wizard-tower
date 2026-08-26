using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

public partial class RoomConstructionPreviewStartingEvent(TowerState towerState, RoomState previewState) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;

    public RoomState RoomState { get; set; } = previewState;
}
