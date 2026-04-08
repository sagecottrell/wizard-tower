using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class FloorConstructionSelectingEvent(TowerState towerState, FloorDefinition floorDefinition) : GodotObject, IDebug, IAllowableEvent, ITowerEvent, IEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public FloorDefinition FloorDefinition { get; } = floorDefinition;
}
