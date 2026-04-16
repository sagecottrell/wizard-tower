using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class FloorConstructionStoppedEvent(TowerState towerState, FloorDefinition floorDefinition) : BaseEvent, IDebug, ITowerEvent, IFloorDefinitionEvent
{
    public TowerState TowerState { get; } = towerState;
    public FloorDefinition FloorDefinition { get; set; } = floorDefinition;
}
