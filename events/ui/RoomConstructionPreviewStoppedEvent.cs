using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class RoomConstructionPreviewStoppedEvent(TowerState towerState) : BaseEvent, IDebug, ITowerEvent
{
    public TowerState TowerState { get; } = towerState;
}
