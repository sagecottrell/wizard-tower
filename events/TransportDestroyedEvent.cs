using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class TransportDestroyedEvent(TowerState towerState, TransportState transportState) : BaseEvent, IEvent, ITowerEvent, ITransportEvent, IDebug
{
    public TowerState TowerState { get; } = towerState;
    public TransportState Transport { get; set; } = transportState;
}
