/**
Generated from ./events/Transport/ui/TransportConstructionPreviewStoppedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport.ui;

public partial class TransportConstructionPreviewStoppingEvent(TowerState towerState) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
}


public static class TransportConstructionPreviewStoppedEventExtensions {
    public static TransportConstructionPreviewStoppedEvent Into(this TransportConstructionPreviewStoppingEvent old) {
        return new(towerState: old.TowerState) { Source = old, };
    }
}