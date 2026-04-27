using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class TransportConstructedEvent(TowerState tower, TransportState transport) : BaseEvent, IDebug, ITowerEvent, ITransportEvent
{
    public TowerState TowerState { get; } = tower;
    public TransportState Transport { get; set; } = transport;
}
