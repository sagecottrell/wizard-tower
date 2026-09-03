/**
Generated from ./events/Floor/ui/FloorExtensionShowedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Floor.ui;

public partial class FloorExtensionShowingEvent(TowerState towerState, FloorDefinition floorDefinition, int elevation, int left, int right) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent, IFloorDefinitionEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public FloorDefinition FloorDefinition { get; set; } = floorDefinition;
    public int Elevation { get; set; } = elevation;
    public int Left { get; set; } = left;
    public int Right { get; set; } = right;
}


public static class FloorExtensionShowedEventExtensions {
    public static FloorExtensionShowedEvent Into(this FloorExtensionShowingEvent old) {
        return new(towerState: old.TowerState, floorDefinition: old.FloorDefinition, elevation: old.Elevation, left: old.Left, right: old.Right) { Source = old, };
    }

    public static FloorExtensionShowingEvent FloorExtensionShowingEvent(this IEvent ev, TowerState towerState, FloorDefinition floorDefinition, int elevation, int left, int right)
    {
        return new(towerState, floorDefinition, elevation, left, right) { Source = ev };
    }
}