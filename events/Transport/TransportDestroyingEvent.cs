/**
Generated from ./events/Transport/TransportDestroyedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Transport;

public partial class TransportDestroyingEvent(TowerState towerState, TransportState transportState) : BaseEvent, IDeniableEvent, IEvent, ITowerEvent, ITransportEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public TransportState TransportState { get; set; } = transportState;
}


public static class TransportDestroyedEventExtensions {
    public static TransportDestroyedEvent Into(this TransportDestroyingEvent old) {
        return new(towerState: old.TowerState, transportState: old.TransportState) { Source = old, };
    }
}