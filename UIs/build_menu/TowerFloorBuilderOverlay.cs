using Godot;
using System;
using wizardtower.actions;
using wizardtower.actions.ui;
using wizardtower.containers;
using wizardtower.events;
using wizardtower.events.interfaces;
using wizardtower.events.ui;
using wizardtower.resource_types;
using wizardtower.state;
using wizardtower.UIs.room_details;
using wizardtower.UIs.transport_details;

namespace wizardtower.UIs.build_menu;

public partial class TowerFloorBuilderOverlay(TowerScript tower) : Node3D(), IUserInterface
{
    public TowerScript Tower { get; set; } = tower;

    private readonly System.Collections.Generic.Dictionary<(int elevation, int position), FloorSelected> _selected = [];

    private FloorDefinition? _currentFloorDef;

    private RichTextLabel _uiLabel = new()
    {
        BbcodeEnabled = true,
        FitContent = true,
        ClipContents = false,
        AutowrapMode = TextServer.AutowrapMode.Off,
    };

    public override void _Ready()
    {
        AddChild(new PanelContainer()
        {
            PivotOffsetRatio = new(0.5f, 0),
            AnchorLeft = 0.5f,
            AnchorRight = 0.5f,
            GrowHorizontal = Control.GrowDirection.Both,
        }.WithChild(_uiLabel));
    }


    public override void _EnterTree()
    {
        GlobalSignals.Singleton.OnFloorConstructionSelected += _onFloorConstructionSelected;
        GlobalSignals.Singleton.OnFloorExtended += _onFloorExtended;
        GlobalSignals.Singleton.OnFloorReplaced += _onFloorReplaced;
        GlobalSignals.Singleton.OnFloorConstructed += _onFloorConstructed;
        GlobalSignals.Singleton.OnShowingUI += _onShowingUI;

        GlobalSignals.Singleton.OnFloorConstructionStopped += _event_reset;
        GlobalSignals.Singleton.OnTransportConstructionSelected += _event_reset;
        GlobalSignals.Singleton.OnRoomConstructionSelected += _event_reset;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnFloorConstructionSelected -= _onFloorConstructionSelected;
        GlobalSignals.Singleton.OnFloorExtended -= _onFloorExtended;
        GlobalSignals.Singleton.OnFloorReplaced -= _onFloorReplaced;
        GlobalSignals.Singleton.OnFloorConstructed -= _onFloorConstructed;
        GlobalSignals.Singleton.OnShowingUI -= _onShowingUI;

        GlobalSignals.Singleton.OnFloorConstructionStopped -= _event_reset;
        GlobalSignals.Singleton.OnTransportConstructionSelected -= _event_reset;
        GlobalSignals.Singleton.OnRoomConstructionSelected -= _event_reset;
    }

    private void _onFloorReplaced(FloorReplacedEvent @event) => _tryStopConstruction(@event.Floor);
    private void _onFloorExtended(FloorExtendedEvent @event) => _tryStopConstruction(@event.Floor);
    private void _onFloorConstructed(FloorConstructedEvent @event) => _tryStopConstruction(@event.Floor);
    private void _event_reset(IEvent @event) => _reset();
    private void _onShowingUI(ShowingUIEvent @event)
    {
        if (_currentFloorDef is null)
            return;
        switch (@event.UserInterface)
        {
            case RoomDetailsUI or TransportDetailsUI: { @event.IsAllowed = false; break; }
        }
    }

    private void _tryStopConstruction(FloorState floor)
    {
        if (GlobalSignals.FloorConstructionStopping(new(Tower.State, floor.Definition)).IsAllowed)
            GlobalSignals.FloorConstructionStopped(new(Tower.State, floor.Definition));
        else
        {
            for (var i = floor.LeftBound; i <= floor.RightBound; i++)
                if (_selected.Remove((floor.Elevation, i), out var s))
                    s.QueueFree();
            _showSelectable();
        }
    }

    private void _onFloorConstructionSelected(FloorConstructionSelectedEvent @event)
    {
        if (_currentFloorDef == @event.FloorDefinition)
            return;
        _reset();
        _currentFloorDef = @event.FloorDefinition;
        _uiLabel.Visible = true;
        _uiLabel.Text = $"Constructing: {_uiLabel.LineHeightImage(_currentFloorDef.Icon)} {_currentFloorDef.Name}";
        _showSelectable();
    }

    private void _reset()
    {
        _uiLabel.Visible = false;
        this.FreeChildren(_selected.Values);
        _selected.Clear();
        _currentFloorDef = null;
    }

    private void _showSelectable()
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
        _showNewTopFloorButton();
        _showNewBasementFloorButton();
    }

    #region Replacement

    private void _showReplacer(FloorState floor)
    {
        if (!_canBuildFloorAt(floor.Elevation))
            return;
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

    #region New floor

    private void _showNewTopFloorButton()
    {
        if (Tower.State.IsHeightLimitReached || !_canBuildFloorAt(Tower.State.HighestFloor + 1))
            return;
        for (int i = Tower.State.DefaultFloorLeftBound; i <= Tower.State.DefaultFloorRightBound; i++)
            _createTile(Tower.State.HighestFloor + 1, i, _onAcceptNewTop);
    }

    private void _showNewBasementFloorButton()
    {
        if (Tower.State.IsDepthLimitReached || !_canBuildFloorAt(Tower.State.LowestFloor - 1))
            return;
        for (int i = Tower.State.DefaultFloorLeftBound; i <= Tower.State.DefaultFloorRightBound; i++)
            _createTile(Tower.State.LowestFloor - 1, i, _onAcceptNewBasement);
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
        UIActions.Hide(new(this));
    }

    private void _onAcceptReplace(int x, int y)
    {
        if (Tower.State.Floors.TryGetValue(y, out var floor) && _currentFloorDef != null)
        {
            Actions.ReplaceFloor(new(Tower.State, floor, _currentFloorDef));
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
            Actions.ExtendFloor(new(Tower.State, floor, left, right));
        }
    }

    private void _onAcceptNewBasement(int x, int y)
    {
        Actions.BuyFloor(new(Tower.State, Tower.State.NewBasementFloor(_currentFloorDef)));
    }

    private void _onAcceptNewTop(int x, int y)
    {
        Actions.BuyFloor(new(Tower.State, Tower.State.NewTopFloor(_currentFloorDef)));
    }

    private bool _canBuildFloorAt(int elevation) => _currentFloorDef?.CanBuildFloorAt(elevation) ?? false;
}
