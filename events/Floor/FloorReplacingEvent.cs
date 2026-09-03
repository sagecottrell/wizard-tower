/**
Generated from ./events/Floor/FloorReplacedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Floor;

public partial class FloorReplacingEvent(TowerState towerState, FloorState floor, FloorDefinition newDefinition) : BaseEvent, IDeniableEvent, IEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public FloorState Floor { get; set; } = floor;
    public FloorDefinition NewDefinition { get; set; } = newDefinition;
}


public static class FloorReplacedEventExtensions {
    public static FloorReplacedEvent Into(this FloorReplacingEvent old) {
        return new(towerState: old.TowerState, floor: old.Floor, newDefinition: old.NewDefinition) { Source = old, };
    }

    public static FloorReplacingEvent FloorReplacingEvent(this IEvent ev, TowerState towerState, FloorState floor, FloorDefinition newDefinition)
    {
        return new(towerState, floor, newDefinition) { Source = ev };
    }
}