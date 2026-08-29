/**
Generated from ./events/Transport/ui/TransportDeselectedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport.ui;

public partial class TransportDeselectingEvent(TowerState tower, TransportState transport) : BaseEvent, IDeniableEvent, ITowerEvent, ITransportEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public TransportState TransportState { get; set; } = transport;
}


public static class TransportDeselectedEventExtensions {
    public static TransportDeselectedEvent Into(this TransportDeselectingEvent old) {
        return new(tower: old.TowerState, transport: old.TransportState) { Source = old, Input = old.Input, };
    }
}