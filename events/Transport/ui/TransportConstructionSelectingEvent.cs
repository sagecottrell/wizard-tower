/**
Generated from ./events/Transport/ui/TransportConstructionSelectedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Transport.ui;

public partial class TransportConstructionSelectingEvent(TowerState towerState, TransportDefinition transportDefinition) : BaseEvent, IDeniableEvent, ITowerEvent, ITransportDefinitionEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public TransportDefinition TransportDefinition { get; set; } = transportDefinition;
}


public static class TransportConstructionSelectedEventExtensions {
    public static TransportConstructionSelectedEvent Into(this TransportConstructionSelectingEvent old) {
        return new(towerState: old.TowerState, transportDefinition: old.TransportDefinition) { Source = old, Input = old.Input, };
    }
}