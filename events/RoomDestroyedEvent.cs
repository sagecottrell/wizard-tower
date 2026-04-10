using Godot;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class RoomDestroyedEvent(TowerState towerState, RoomState room, IEvent source) : GodotObject, IEvent, ITowerEvent, IDebug
{
    public TowerState TowerState { get; } = towerState;
    public RoomState Room { get; } = room;
    public IEvent Source { get; } = source;
}
