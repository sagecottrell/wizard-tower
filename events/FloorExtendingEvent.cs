using Godot;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class FloorExtendingEvent(TowerState towerState, FloorState floor, int left, int right) : GodotObject, IDebug, ITowerEvent, IAllowableEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public FloorState Floor { get; } = floor;
    public int Left { get; } = left;
    public int Right { get; } = right;
}
