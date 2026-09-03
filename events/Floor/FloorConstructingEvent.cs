/**
Generated from ./events/Floor/FloorConstructedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Floor;

public partial class FloorConstructingEvent(TowerState tower, FloorState floor) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent, IEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public FloorState Floor { get; set; } = floor;
}


public static class FloorConstructedEventExtensions {
    public static FloorConstructedEvent Into(this FloorConstructingEvent old) {
        return new(tower: old.TowerState, floor: old.Floor) { Source = old, };
    }

    public static FloorConstructingEvent FloorConstructingEvent(this IEvent ev, TowerState tower, FloorState floor)
    {
        return new(tower, floor) { Source = ev };
    }
}