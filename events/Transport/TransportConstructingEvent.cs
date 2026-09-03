/**
Generated from ./events/Transport/TransportConstructedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport;

public partial class TransportConstructingEvent(TowerState tower, TransportState transport) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent, ITransportEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public TransportState TransportState { get; set; } = transport;
}


public static class TransportConstructedEventExtensions {
    public static TransportConstructedEvent Into(this TransportConstructingEvent old) {
        return new(tower: old.TowerState, transport: old.TransportState) { Source = old, };
    }

    public static TransportConstructingEvent TransportConstructingEvent(this IEvent ev, TowerState tower, TransportState transport)
    {
        return new(tower, transport) { Source = ev };
    }
}