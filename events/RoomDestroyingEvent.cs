using Godot;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class RoomDestroyingEvent(TowerState towerState, RoomState room, IEvent source) : GodotObject, IEvent, ITowerEvent, IDebug, IAllowableEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public RoomState Room { get; } = room;
    public IEvent Source { get; } = source;
}
