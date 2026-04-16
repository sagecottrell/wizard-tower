using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class TransportConstructionSelectingEvent(TowerState towerState, TransportDefinition transportDefinition) : BaseEvent, ITowerEvent, IAllowableEvent, ITransportDefinitionEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public TransportDefinition TransportDefinition { get; set; } = transportDefinition;
}
