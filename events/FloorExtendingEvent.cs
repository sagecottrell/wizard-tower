using Godot;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class FloorExtendingEvent(TowerState towerState, FloorState floor, uint extendedLeft, uint extendedRight) : GodotObject, IDebug, ITowerEvent, IAllowableEvent, IEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public FloorState Floor { get; } = floor;
    public uint ExtendedLeft { get; } = extendedLeft;
    public uint ExtendedRight { get; } = extendedRight;
    public uint ExtensionAmount => ExtendedLeft + ExtendedRight;
}
