using Godot;
using System.Collections.Generic;
using System.Linq;
using wizardtower.state;

namespace wizardtower.containers;

[GlobalClass]
public partial class TransportScript : Node3D
{
    public TransportState State { get; set; } = new();
    public TransportState PreviousState { get; set; } = new();

    private List<TransportBackgroundScript> TransportVisualNodes { get; set; } = [];

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
        if (State.Definition.Scene is not PackedScene scene)
            return;

        if (PreviousState.Definition.Scene != State.Definition.Scene)
        {
            this.FreeChildren(TransportVisualNodes);
            for (var i = 0; i < State.Height; i++)
            {
                if (scene.Instantiate() is not TransportBackgroundScript node)
                    break;
                AddChild(node);
                TransportVisualNodes.Add(node);
                node.Position = new(0, i, 0);
            }
        }

        if (State.Height < TransportVisualNodes.Count)
        {
            var removed = TransportVisualNodes[(int)State.Height..];
            TransportVisualNodes = [.. TransportVisualNodes.Take((int)State.Height)];
            this.FreeChildren(removed);
        }
        else if (State.Height > TransportVisualNodes.Count)
        {
            for (var i = TransportVisualNodes.Count; i < State.Height; i++)
            {
                if (scene.Instantiate() is not TransportBackgroundScript node)
                    break;
                AddChild(node);
                TransportVisualNodes.Add(node);
                node.Position = new(0, i, 0);
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
        foreach (var node in TransportVisualNodes)
            node.AsHologram();
        return this;
    }

    public TransportScript AsBackground()
    {
        HologramMode = false;
        foreach (var node in TransportVisualNodes)
            node.AsBackground();
        return this;
    }
}
