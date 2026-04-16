using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class FloorConstructionSelectingEvent(TowerState towerState, FloorDefinition floorDefinition) : BaseEvent, IDebug, IAllowableEvent, ITowerEvent, IFloorDefinitionEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public FloorDefinition FloorDefinition { get; set; } = floorDefinition;
}
