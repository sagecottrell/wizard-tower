using Godot;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class FloorConstructedEvent(TowerState tower, FloorState floor) : GodotObject, IDebug, ITowerEvent, IEvent
{
    public TowerState TowerState { get; } = tower;
    public FloorState Floor { get; } = floor;
}
