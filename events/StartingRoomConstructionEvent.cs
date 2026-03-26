using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;

namespace wizardtower.events;

public partial class StartingRoomConstructionEvent(RoomDefinition roomDefinition) : GodotObject, IAllowableEvent, IDebug
{
    public bool IsAllowed { get; set; } = true;

    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
}
