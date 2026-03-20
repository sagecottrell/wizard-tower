using Godot;
using Godot.Collections;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class TowerState : Resource
{
    [Export]
    public Dictionary<int, FloorState> Floors { get; set; } = [];

    [Export]
    public Dictionary<uint, RoomState> Rooms { get; set; } = [];

    [Export]
    public uint RoomIdCounter { get; set; } = 0;

    [Export]
    public int LowestFloor { get; set; }

    [Export]
    public int HighestFloor { get; set; }
}
