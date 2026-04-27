using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class TransportDeselectedEvent(TowerState tower, TransportState transport) : BaseEvent, ITowerEvent, ITransportEvent, IDebug
{
    public TowerState TowerState { get; } = tower;
    public TransportState Transport { get; set; } = transport;
}
