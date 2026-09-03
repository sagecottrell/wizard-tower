/**
Generated from ./events/Floor/ui/FloorConstructionSelectedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Floor.ui;

public partial class FloorConstructionSelectingEvent(TowerState towerState, FloorDefinition floorDefinition) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent, IFloorDefinitionEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public FloorDefinition FloorDefinition { get; set; } = floorDefinition;
}


public static class FloorConstructionSelectedEventExtensions {
    public static FloorConstructionSelectedEvent Into(this FloorConstructionSelectingEvent old) {
        return new(towerState: old.TowerState, floorDefinition: old.FloorDefinition) { Source = old, };
    }

    public static FloorConstructionSelectingEvent FloorConstructionSelectingEvent(this IEvent ev, TowerState towerState, FloorDefinition floorDefinition)
    {
        return new(towerState, floorDefinition) { Source = ev };
    }
}