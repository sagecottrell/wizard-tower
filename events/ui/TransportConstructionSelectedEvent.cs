using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class TransportConstructionSelectedEvent(TowerState towerState, TransportDefinition transportDefinition) : BaseEvent, ITowerEvent, ITransportDefinitionEvent, IDebug
{
    public TowerState TowerState { get; set; } = towerState;
    public TransportDefinition TransportDefinition { get; set; } = transportDefinition;
}
