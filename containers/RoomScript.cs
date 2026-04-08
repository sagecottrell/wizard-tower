using Godot;
using wizardtower.state;

namespace wizardtower.containers;

[Tool]
[GlobalClass]
public partial class RoomScript : Node3D
{
    public RoomState State { get; set; } = new();

    public RoomState PreviousState { get; set; } = new();

    private RoomBackgroundScript? RoomScene { get; set; }

    public bool HologramMode { get; set; } = false;

    public override void _Ready()
    {
        if (HologramMode)
            AsHologram();
         else
            AsBackground();
    }

    public override void _Process(double delta)
    {
        if (State != PreviousState && State.Compare(PreviousState))
            return;

        if (PreviousState.Definition.RoomScene != State.Definition.RoomScene)
        {
            RoomScene?.QueueFree();
            if (State.Definition.RoomScene is PackedScene scene)
            {
                RoomScene = scene.Instantiate() as RoomBackgroundScript;
                AddChild(RoomScene);
            }
        }

        if (HologramMode)
            AsHologram();
         else
            AsBackground();

        Position = this.TowerCoordToNodePosition(x: State.FloorPosition, y: State.Elevation);

        PreviousState = State.Copy();
    }

    public RoomScript AsHologram()
    {
        HologramMode = true;
        RoomScene?.AsHologram();
        return this;
    }

    public RoomScript AsBackground()
    {
        HologramMode = false;
        RoomScene?.AsBackground();
        return this;
    }
}
