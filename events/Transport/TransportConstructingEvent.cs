using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport;

public partial class TransportConstructingEvent(TowerState tower, TransportState transport) : BaseEvent, IDebug, ITowerEvent, ITransportEvent, IDeniableEvent, IEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public TransportState TransportState { get; set; } = transport;
}
