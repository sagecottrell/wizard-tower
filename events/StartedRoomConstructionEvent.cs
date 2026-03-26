using Godot;
using System.Diagnostics;
using wizardtower.resource_types;

namespace wizardtower.events;

[DebuggerDisplay("Def={RoomDefinition.Name}")]
public partial class StartedRoomConstructionEvent(RoomDefinition roomDefinition) : GodotObject, IDebug
{
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
}
