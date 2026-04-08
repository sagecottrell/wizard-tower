using Godot;
using wizardtower.state;

namespace wizardtower.containers;

[GlobalClass]
public partial class TransportScript : Node3D
{
    public TransportState State { get; set; } = new();
    public TransportState PreviousState { get; set; } = new();

    private TransportBackgroundScript? TransportVisualNode { get; set; }

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

        if (PreviousState.Definition.Scene != State.Definition.Scene)
        {
            TransportVisualNode?.QueueFree();
            if (State.Definition.Scene is PackedScene scene)
            {
                TransportVisualNode = scene.Instantiate() as TransportBackgroundScript;
                AddChild(TransportVisualNode);
            }
        }

        if (HologramMode)
            AsHologram();
        else
            AsBackground();

        Position = this.TowerCoordToNodePosition(x: State.HorizontalPosition, y: State.Elevation);

        PreviousState = State.Copy();
    }

    public TransportScript AsHologram()
    {
        HologramMode = true;
        TransportVisualNode?.AsHologram();
        return this;
    }

    public TransportScript AsBackground()
    {
        HologramMode = false;
        TransportVisualNode?.AsBackground();
        return this;
    }
}
