using Godot;
using wizardtower.state;

namespace wizardtower;

[Tool]
[GlobalClass]
public partial class RoomScript : Node3D
{
    [Export]
    public RoomState State { get; set; } = new();

    public RoomState PreviousState { get; set; } = new();

    public override void _Ready()
    {
        if (State == PreviousState)
            return;
    }
}
