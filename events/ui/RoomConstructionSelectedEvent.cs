using Godot;
using System.Diagnostics;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.ui;

[DebuggerDisplay("Def={RoomDefinition.Name}")]
public partial class RoomConstructionSelectedEvent(TowerState tower, RoomDefinition roomDefinition) : GodotObject, IDebug, ITowerEvent, IEvent
{
    public TowerState TowerState { get; } = tower;
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
}
