using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class RoomConstructionPreviewEvent(TowerState towerState, RoomState previewState) : BaseEvent, IDebug, ITowerEvent, IRoomEvent
{
    public TowerState TowerState { get; } = towerState;

    public RoomState Room { get; set; } = previewState;
}
