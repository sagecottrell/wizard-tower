using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport;

public partial class TransportDestroyingEvent(TowerState towerState, TransportState transportState) : BaseEvent, IEvent, ITowerEvent, ITransportEvent, IDebug, IDeniableEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public TransportState TransportState { get; set; } = transportState;
}
