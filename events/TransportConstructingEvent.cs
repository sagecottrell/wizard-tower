using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class TransportConstructingEvent(TowerState tower, TransportState transport) : BaseEvent, IDebug, ITowerEvent, ITransportEvent, IAllowableEvent, IEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public TransportState Transport { get; set; } = transport;
}
