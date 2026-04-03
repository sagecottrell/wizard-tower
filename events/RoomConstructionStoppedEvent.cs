using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class RoomConstructionStoppedEvent(TowerState tower, RoomDefinition roomDefinition, bool userRequested = false) : GodotObject, IDebug, ITowerEvent
{
    public TowerState TowerState { get; } = tower;
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
    public bool UserRequested { get; } = userRequested;
}
