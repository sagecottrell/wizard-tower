using Godot;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class RoomConstructedEvent(TowerState tower, RoomDefinition room, int elevation, int pos) : GodotObject, IDebug
{
    public TowerState Tower { get; } = tower;
    public RoomDefinition Room { get; } = room;
    public int Elevation { get; } = elevation;
    public int Pos { get; } = pos;
}
