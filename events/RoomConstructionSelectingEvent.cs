using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class RoomConstructionSelectingEvent(TowerState towerState, RoomDefinition roomDefinition) : GodotObject, IAllowableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
}
