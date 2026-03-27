using Godot;
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

    public override void _Ready()
    {
        Name = $"Floor{State.Elevation}";
        Position = this.TowerCoordToNodePosition(y: State.Elevation);
    }

    public override void _Process(double delta)
    {
        if (_tiles.Count == 0)
            foreach (var child in GetChildren(false))
            {
                if (child.Owner is null)
                {
                    if (child is FloorBackgroundTile fbt)
                        _tiles.Add(fbt.Index, fbt);
                    else
                        child.QueueFree();
                }
                    
            }

        Position = Position.MoveToward(this.TowerCoordToNodePosition(y: State.Elevation), (float)delta);

        if (State.Compare(PreviousState))
            return;

        SetupTiles();
        PreviousState = State.Copy();
    }

    public void SetupTiles()
    {
        foreach (var id in _tiles.Keys)
        {
            if (id < State.LeftBound || id > State.RightBound)
                _removeTile(id);
        }

        for (var i = State.LeftBound; i <= State.RightBound; i++)
        {
            _addTile(i);
        }
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

    private FloorBackgroundTile? _addTile(int i)
    {
        if (State.Definition.FloorBackgroundTileScene is not PackedScene scene)
            return null;
        if (_tiles.TryGetValue(i, out var tile))
            return tile;
        if (scene.Instantiate() is FloorBackgroundTile fbt && _tiles.TryAdd(i, fbt))
        {
            AddChild(fbt);
            fbt.Index = i;
            fbt.Position = fbt.TowerCoordToNodePosition(x: i);
            fbt.OnCreate();
            return fbt;
        }
        return null;
    }

    public void SetPositionVisible(int left, uint width, bool visible)
    {
        for (var i = 0; i < width; i++)
        {
            _addTile(i + left)?.WallVisible(visible);
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

    public void Destroy()
    {
        QueueFree();
    }
}
