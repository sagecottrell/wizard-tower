using Godot;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class RoomConstructedEvent(TowerState tower, RoomState room) : GodotObject, IDebug, ITowerEvent
{
    public TowerState TowerState { get; } = tower;
    public RoomState Room { get; } = room;
}
