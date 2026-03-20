using Godot;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class RoomState : Resource
{
    [Export]
    public uint Id { get; set; }

    [Export]
    public int Elevation { get; set; }

    [Export]
    public uint Height { get; set; }

    [Export]
    public int FloorPosition { get; set; }
}
