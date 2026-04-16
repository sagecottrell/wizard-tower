using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class TransportConstructionPreviewEvent(TowerState towerState, TransportState? previewState) : BaseEvent, IDebug, ITowerEvent
{
    public TowerState TowerState { get; } = towerState;

    // if null, then no preview is being shown
    public TransportState? PreviewState { get; } = previewState;
}
