using Godot;
using System;
using wizardtower.containers;
using wizardtower.events;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;

public partial class TowerFloorBuilderOverlay(TowerScript tower) : Node3D()
{
    [Signal]
    public delegate void OnFloorConstructEventHandler(FloorConstructingEvent @event);
    [Signal]
    public delegate void OnFloorReplaceEventHandler(FloorReplacingEvent @event);
    [Signal]
    public delegate void OnFloorExtendEventHandler(FloorExtendingEvent @event);

    public TowerScript Tower { get; set; } = tower;

    private readonly System.Collections.Generic.Dictionary<(int elevation, int position), FloorSelected> _selected = [];

    private FloorDefinition? _currentFloorDef;

    public override void _Ready()
    {
        if (GlobalSignals.Singleton is GlobalSignals g)
        {
            g.OnFloorConstructionSelected += _onFloorConstructionSelected;
            g.OnFloorConstructionStopped += _onFloorConstructionStopped;
            g.OnFloorExtended += _onFloorExtended;
            g.OnFloorReplaced += _onFloorReplaced;
        }
    }

    private void _onFloorReplaced(FloorReplacedEvent @event) => _tryStopConstruction(@event.Floor);

    private void _onFloorExtended(FloorExtendedEvent @event) => _tryStopConstruction(@event.Floor);

    private void _tryStopConstruction(FloorState floor)
    {
        if (GlobalSignals.FloorConstructionStopping(new(Tower.State, floor.Definition)).IsAllowed)
            GlobalSignals.FloorConstructionStopped(new(Tower.State, floor.Definition));
        else
        {
            for (var i = floor.LeftBound; i <= floor.RightBound; i++)
                if (_selected.Remove((floor.Elevation, i), out var s))
                    s.QueueFree();
            _showExtenders();
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
        _currentFloorDef = @event.FloorDefinition;
        this.FreeChildren(_selected.Values);
        _selected.Clear();
        _showExtenders();
    }

    private void _showExtenders()
    {
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

    #region Replacement

    private void _showReplacer(FloorState floor)
    {
        for (int i = floor.LeftBound; i <= floor.RightBound; i++)
            _createTile(floor.Elevation, i, _onAcceptReplace);
    }

    #endregion

    #region Extension

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
            this.Error($"Floor at elevation {floor.Elevation} extension bounds cannot be determined; cannot be extended");
        }
    }

    private void _matchWidth(FloorState floor, int left, int right)
    {
        for (int i = left; i < floor.LeftBound; i++)
            _createTile(floor.Elevation, i, _onAcceptExtend);
        for (int i = floor.RightBound + 1; i <= right; i++)
            _createTile(floor.Elevation, i, _onAcceptExtend);
    }

    #endregion

    private FloorSelected _createTile(int y, int x, Action<int, int> onAccept)
    {
        if (_selected.TryGetValue((y, x), out var existing))
            return existing;
        if (!SceneLoader.TryLoadScene<FloorSelected>(out var tile))
            throw new Exception("Failed to load FloorSelected scene");

        AddChild(tile);
        _selected[(y, x)] = tile;
        tile.Position = tile.TowerCoordToNodePosition(x, y);
        //tile.OnMouseEntered += _ => _onMouseEnter(x, y);
        tile.OnAccept += _ => onAccept(x, y);
        tile.OnCancel += _ => _onCancel(x, y);
        return tile;
    }

    private void _onCancel(int x, int y)
    {
    }

    private void _onAcceptReplace(int x, int y)
    {
        if (Tower.State.Floors.TryGetValue(y, out var floor) && _currentFloorDef != null)
        {
            EmitSignalOnFloorReplace(new(Tower.State, floor, _currentFloorDef));
        }
    }

    private void _onAcceptExtend(int x, int y)
    {
        if (Tower.State.Floors.TryGetValue(y, out var floor))
        {
            // extending the floor
            uint left = 0;
            uint right = 0;
            if (x < floor.LeftBound)
            {
                left = (uint)(floor.LeftBound - x);
            }
            else if (x > floor.RightBound)
            {
                right = (uint)(x - floor.RightBound);
            }
            else
            {
                this.Error($"Clicked on an existing part of the floor at ({x}, {y}), this should not be possible");
                return;
            }
            EmitSignalOnFloorExtend(new(Tower.State, floor, left, right));
        }
    }
}
