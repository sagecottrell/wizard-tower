/**
Generated from ./events/Floor/FloorExtendedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Floor;

public partial class FloorExtendingEvent(TowerState towerState, FloorState floor, uint extendedLeft, uint extendedRight) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent, IEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public FloorState Floor { get; set; } = floor;
    public uint ExtendedLeft { get; set; } = extendedLeft;
    public uint ExtendedRight { get; set; } = extendedRight;
    public uint ExtensionAmount => ExtendedLeft + ExtendedRight;
}


public static class FloorExtendedEventExtensions {
    public static FloorExtendedEvent Into(this FloorExtendingEvent old) {
        return new(towerState: old.TowerState, floor: old.Floor, extendedLeft: old.ExtendedLeft, extendedRight: old.ExtendedRight) { Source = old, };
    }
}