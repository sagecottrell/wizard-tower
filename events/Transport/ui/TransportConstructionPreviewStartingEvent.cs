using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport.ui;

public partial class TransportConstructionPreviewStartingEvent(TowerState towerState, TransportState? previewState) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;

    // if null, then no preview is being shown
    public TransportState? PreviewState { get; set; } = previewState;
}
