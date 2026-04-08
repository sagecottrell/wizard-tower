using Godot;
using System;
using wizardtower.events;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;

public partial class TowerFloorBuilderOverlay(TowerScript tower) : Node3D()
{
    [Signal]
    public delegate void OnFloorConstructingEventHandler(FloorState room);

    [Signal]
    public delegate void OnFloorExtendingEventHandler(FloorState room, int leftBound, int rightBound);

    public TowerScript Tower { get; set; } = tower;

    private readonly System.Collections.Generic.Dictionary<(int elevation, int position), FloorSelected> _selected = [];

    private FloorDefinition? _currentFloorDef;

    public override void _Ready()
    {
        if (GlobalSignals.Singleton is GlobalSignals g)
        {
            g.OnFloorConstructionSelected += _onFloorConstructionSelected;
            g.OnFloorConstructionStopped += _onFloorConstructionStopped;
        }
    }

    private void _onFloorConstructionStopped(FloorConstructionStoppedEvent @event)
    {
        this.FreeChildren(_selected.Values);
        _selected.Clear();
        _currentFloorDef = null;
    }

    private void _onFloorConstructionSelected(FloorConstructionSelectedEvent @event)
    {
        if (@event.TowerState != Tower.State)
            return;

        if (_currentFloorDef is not null)
        {
            GlobalSignals.FloorConstructionStopped(new(Tower.State, _currentFloorDef));
            return;
        }

        _currentFloorDef = @event.FloorDefinition;
        this.FreeChildren(_selected.Values);
        _selected.Clear();

        // floors that match this floor def can be extended
        // floors that do not match this floor def can be replaced
        foreach (var floor in Tower.State.Floors.Values)
        {
            if (floor.Definition == _currentFloorDef)
                _showExtenders(floor);
            else
                _showReplacer(floor); 
        }
    }

    private void _showExtenders(FloorState floor)
    {
        if (floor.Elevation == 0)
        {
            // ground floor can always extend up to the maximum floor width
            _matchWidth(floor, -(int)Tower.State.MaxWidth, (int)Tower.State.MaxWidth);
        }
        else if (floor.Elevation < 0 && Tower.State.Floors.TryGetValue(0, out var groundFloor))
        {
            // basement floors can extend up to the width of the ground floor
            _matchWidth(floor, groundFloor.LeftBound, groundFloor.RightBound);
        }
        else if (floor.Elevation > 0 && Tower.State.Floors.TryGetValue(floor.Elevation - 1, out var below))
        {
            // above-ground floors can extend up to the width of the floor below them
            _matchWidth(floor, below.LeftBound, below.RightBound);
        }
        else
        {
            this.Error($"Floor at elevation {floor.Elevation} has no floor below it, cannot be extended");
        }
    }

    private void _showReplacer(FloorState floor)
    {
    }

    private void _matchWidth(FloorState floor, int left, int right)
    {
        for (int i = left; i < floor.LeftBound; i++)
            _createTile(floor.Elevation, i);
        for (int i = floor.RightBound + 1; i <= right; i++)
            _createTile(floor.Elevation, i);
    }

    private FloorSelected _createTile(int y, int x)
    {
        if (!SceneLoader.TryLoadScene<FloorSelected>(out var tile))
            throw new Exception("Failed to load FloorSelected scene");

        AddChild(tile);
        _selected[(y, x)] = tile;
        tile.Position = tile.TowerCoordToNodePosition(x, y);
        //tile.OnMouseEntered += _ => _onMouseEnter(x, y);
        tile.OnAccept += _ => _onAccept(x, y);
        tile.OnCancel += _ => _onCancel(x, y);
        return tile;
    }

    private void _onCancel(int x, int y)
    {
        throw new NotImplementedException();
    }

    private void _onAccept(int x, int y)
    {
        if (Tower.State.Floors.TryGetValue(y, out var floor)) {
            // extending the floor
            var left = floor.LeftBound;
            var right = floor.RightBound;
            if (x < floor.LeftBound)
            {
                left = x;
            }
            else if (x > floor.RightBound) {
                right = x;
            }
            else
            {
                this.Error($"Clicked on an existing part of the floor at ({x}, {y}), this should not be possible");
                return;
            }

            if (GlobalSignals.FloorExtending(new(Tower.State, floor, left, right)).IsAllowed)
            {
                EmitSignalOnFloorExtending(floor, left, right);
                GlobalSignals.FloorExtended(new(Tower.State, floor, left, right));

                if (GlobalSignals.FloorConstructionStopping(new(Tower.State, floor.Definition)).IsAllowed)
                    GlobalSignals.FloorConstructionStopped(new(Tower.State, floor.Definition));
                else
                {
                    for (var i = left; i <= right; i++)
                        if (_selected.Remove((y, i), out var s))
                            s.QueueFree();
                }
            }
        }
    }
}
