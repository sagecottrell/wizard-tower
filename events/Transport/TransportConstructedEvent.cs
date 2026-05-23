using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport;

public partial class TransportConstructedEvent(TowerState tower, TransportState transport) : BaseEvent, IDebug, ITowerEvent, ITransportEvent
{
    public TowerState TowerState { get; } = tower;
    public TransportState TransportState { get; set; } = transport;
}
