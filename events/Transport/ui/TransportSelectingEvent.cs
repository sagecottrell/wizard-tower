using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport.ui;

public partial class TransportSelectingEvent(TowerState tower, TransportState transport) : BaseEvent, ITowerEvent, IDeniableEvent, ITransportEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public TransportState Transport { get; set; } = transport;
}
