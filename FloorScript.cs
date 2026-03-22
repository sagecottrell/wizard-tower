using Godot;
using System;
using System.Collections.Generic;
using wizardtower.state;

namespace wizardtower;

[Tool]
[GlobalClass]
public partial class FloorScript : Node3D
{
    [Export]
    public FloorState State { get; set; } = new();

    public FloorState PreviousState { get; set; } = new();

    private readonly Dictionary<int, FloorBackgroundTile> _tiles = [];

    public override void _Process(double delta)
    {
        if (State.Definition is null || State.Definition.FloorBackgroundTileScene is null)
            return;

        Position = Position.MoveToward(this.TowerCoordToNodePosition(y: State.Elevation), (float)delta);

        if (State == PreviousState)
            return;

        var scene = State.Definition.FloorBackgroundTileScene;

        if (State.SizeLeft < PreviousState.SizeLeft)
            for (uint i = State.SizeLeft + 1; i <= PreviousState.SizeLeft; i++)
                _removeTile(-(int)i);
        if (State.SizeLeft > PreviousState.SizeLeft)
            for (uint i = Math.Max(1, PreviousState.SizeLeft); i <= State.SizeLeft; i++)
                _addTile(-(int)i, scene);
        if (State.SizeRight < PreviousState.SizeRight)
            for (uint i = State.SizeRight + 1; i <= PreviousState.SizeRight; i++)
                _removeTile((int)i);
        if (State.SizeRight > PreviousState.SizeRight)
            for (uint i = Math.Max(1, PreviousState.SizeRight); i <= State.SizeRight; i++)
                _addTile((int)i, scene);

        PreviousState = State.Copy();
    }

    private void _removeTile(int i)
    {
        if (_tiles.Remove(i, out var tile))
        {
            // TODO: peform animation before removing child
            RemoveChild(tile);
            tile.QueueFree();
        }
    }

    private void _addTile(int i, PackedScene scene)
    {
        if (scene.Instantiate() is FloorBackgroundTile fbt && _tiles.TryAdd(i, fbt))
        {
            AddChild(fbt);
            fbt.Position = fbt.TowerCoordToNodePosition(x: i);
            fbt.OnCreate();
        }
    }


    [ExportToolButton("OnCreate")]
    public Callable TriggerOnCreate => Callable.From(OnCreate);

    public void OnCreate()
    {
        if (State.Elevation != 0)
            PreviousState.Elevation = State.Elevation - System.Math.Sign(State.Elevation);
        else
            PreviousState.Elevation = -1;
        Position = this.TowerCoordToNodePosition(y: PreviousState.Elevation);
    }
}
