using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport.ui;

public partial class TransportDeselectedEvent(TowerState tower, TransportState transport) : BaseEvent, ITowerEvent, ITransportEvent, IDebug
{
    public TowerState TowerState { get; } = tower;
    public TransportState TransportState { get; set; } = transport;
}
