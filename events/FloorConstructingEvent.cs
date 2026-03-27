using Godot;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class FloorConstructingEvent(TowerState tower, FloorState floor) : GodotObject, IDebug, ITowerEvent, IAllowableEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public FloorState Floor { get; } = floor;
}
