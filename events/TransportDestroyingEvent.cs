using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class TransportDestroyingEvent(TowerState towerState, TransportState transportState) : BaseEvent, IEvent, ITowerEvent, ITransportEvent, IDebug, IAllowableEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public TransportState Transport { get; set; } = transportState;
}
