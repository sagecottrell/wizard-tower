/**
Generated from ./events/Floor/ui/FloorConstructionStoppedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Floor.ui;

public partial class FloorConstructionStoppingEvent(TowerState towerState, FloorDefinition floorDefinition) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent, IFloorDefinitionEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public FloorDefinition FloorDefinition { get; set; } = floorDefinition;
}


public static class FloorConstructionStoppedEventExtensions {
    public static FloorConstructionStoppedEvent Into(this FloorConstructionStoppingEvent old) {
        return new(towerState: old.TowerState, floorDefinition: old.FloorDefinition) { Source = old, };
    }
}