/**
Generated from ./events/Transport/ui/TransportConstructionStoppedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Transport.ui;

public partial class TransportConstructionStoppingEvent(TowerState towerState, TransportDefinition transportDefinition) : BaseEvent, IDeniableEvent, ITowerEvent, ITransportDefinitionEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public TransportDefinition TransportDefinition { get; set; } = transportDefinition;
}


public static class TransportConstructionStoppedEventExtensions {
    public static TransportConstructionStoppedEvent Into(this TransportConstructionStoppingEvent old) {
        return new(towerState: old.TowerState, transportDefinition: old.TransportDefinition) { Source = old, };
    }
}