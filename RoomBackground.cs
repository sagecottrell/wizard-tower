using Godot;
using wizardtower.state;

namespace wizardtower;

[Tool]
public partial class RoomBackground : Node3D
{
    public RoomState RoomState { get; set; } = new();

    [Export]
    public Node3D? SomethingAnimatable { get; set; }

    public void DoSomething() { }
}
