using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class FloorConstructionStoppedEvent(TowerState towerState, FloorDefinition floorDefinition) : GodotObject, IDebug, ITowerEvent, IEvent
{
    public TowerState TowerState { get; } = towerState;
    public FloorDefinition FloorDefinition { get; } = floorDefinition;
}
