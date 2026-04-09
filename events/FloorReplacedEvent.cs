using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class FloorReplacedEvent(TowerState towerState, FloorState floor, FloorDefinition newDefinition) : GodotObject, IEvent, IDebug, ITowerEvent
{
    public TowerState TowerState { get; } = towerState;
    public FloorState Floor { get; } = floor;
    public FloorDefinition NewDefinition { get; } = newDefinition;
}
