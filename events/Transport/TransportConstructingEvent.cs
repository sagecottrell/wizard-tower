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