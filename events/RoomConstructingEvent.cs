using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class RoomConstructingEvent(TowerState tower, RoomDefinition room, int elevation, int pos) : GodotObject, IAllowableEvent
{
    public bool IsAllowed { get; set; }
    public TowerState Tower { get; } = tower;
    public RoomDefinition Room { get; } = room;
    public int Elevation { get; } = elevation;
    public int Pos { get; } = pos;
}
