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

    private Node? RoomScene { get; set; }

    public override void _Process(double delta)
    {
        if (State != PreviousState && State.Compare(PreviousState))
            return;

        if (PreviousState.Definition.RoomScene != State.Definition.RoomScene)
        {
            RoomScene?.QueueFree();
            if (State.Definition.RoomScene is PackedScene scene)
            {
                RoomScene = scene.Instantiate();
                AddChild(RoomScene);
            }
        }

        Position = this.TowerCoordToNodePosition(x: State.FloorPosition, y: State.Elevation);

        PreviousState = State.Copy();
    }
}
