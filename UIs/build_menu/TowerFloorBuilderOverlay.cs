using Godot;
using System;
using wizardtower.actions;
using wizardtower.actions.ui;
using wizardtower.containers;
using wizardtower.events.Floor;
using wizardtower.events.Floor.ui;
using wizardtower.events.handlers;
using wizardtower.events.Interface;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;
using wizardtower.UIs.room_details;
using wizardtower.UIs.selector;
using wizardtower.UIs.transport_details;

namespace wizardtower.UIs.build_menu;

public partial class TowerFloorBuilderOverlay(TowerScript tower) : Node3D(), IUserInterface
{
    public TowerScript Tower { get; set; } = tower;

    private readonly System.Collections.Generic.Dictionary<(int elevation, int position), Selector> _selected = [];

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
        FloorEvents.Ui.ConstructionSelected += _onFloorConstructionSelected;
        FloorEvents.Extended += _onFloorExtended;
        FloorEvents.Replaced += _onFloorReplaced;
        FloorEvents.Constructed += _onFloorConstructed;
        InterfaceEvents.Showing += _onShowingUI;

        FloorEvents.Ui.ConstructionStopped += _event_reset;
        TransportEvents.Ui.ConstructionSelected += _event_reset;
        RoomEvents.Ui.ConstructionSelected += _event_reset;
    }

    public override void _ExitTree()
    {
        FloorEvents.Ui.ConstructionSelected -= _onFloorConstructionSelected;
        FloorEvents.Extended -= _onFloorExtended;
        FloorEvents.Replaced -= _onFloorReplaced;
        FloorEvents.Constructed -= _onFloorConstructed;
        InterfaceEvents.Showing -= _onShowingUI;
        FloorEvents.Ui.ConstructionStopped -= _event_reset;
        TransportEvents.Ui.ConstructionSelected -= _event_reset;
        RoomEvents.Ui.ConstructionSelected -= _event_reset;
    }

    private void _onFloorReplaced(FloorReplacedEvent @event) => _tryStopConstruction(@event.Floor);
    private void _onFloorExtended(FloorExtendedEvent @event) => _tryStopConstruction(@event.Floor);
    private void _onFloorConstructed(FloorConstructedEvent @event) => _tryStopConstruction(@event.Floor);
    private void _event_reset(IEvent @event) => _reset();
    private void _onShowingUI(InterfaceShowingEvent @event)
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
        if (FloorEvents.Ui.TryConstructionStopping(new(Tower.State, floor.Definition), out var e))
            FloorEvents.Ui.OnConstructionStopped(e);
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
        if (FloorEvents.Ui.TryExtensionShowing(new(Tower.State, floor.Definition, floor.Elevation, left, right), out var e))
        {
            for (int i = e.Left; i < floor.LeftBound; i++)
                _createTile(e.Elevation, i, _onAcceptExtend);
            for (int i = floor.RightBound + 1; i <= e.Right; i++)
                _createTile(e.Elevation, i, _onAcceptExtend);
            FloorEvents.Ui.OnExtensionShowed(e);
        }
    }

    #endregion

    #region New floor

    private void _showNewTopFloorButton()
    {
        if (Tower.State.IsHeightLimitReached || !_canBuildFloorAt(Tower.State.HighestFloor + 1))
            return;
        _createTile(Tower.State.HighestFloor + 1, Tower.State.DefaultFloorLeftBound, _onAcceptNewTop, right: (uint)(Tower.State.DefaultFloorRightBound - Tower.State.DefaultFloorLeftBound) - 1);
    }

    private void _showNewBasementFloorButton()
    {
        if (Tower.State.IsDepthLimitReached || !_canBuildFloorAt(Tower.State.LowestFloor - 1))
            return;
        _createTile(Tower.State.LowestFloor - 1, Tower.State.DefaultFloorLeftBound, _onAcceptNewBasement, right: (uint)(Tower.State.DefaultFloorRightBound - Tower.State.DefaultFloorLeftBound) - 1);
    }

    #endregion

    private Selector _createTile(int y, int x, Action<int, int, UserEvent> onAccept, uint? right = null)
    {
        if (_selected.TryGetValue((y, x), out var existing))
            return existing;
        if (!SceneLoader.TryLoadScene<Selector>(out var tile))
            throw new Exception("Failed to load FloorSelected scene");

        AddChild(tile);
        _selected[(y, x)] = tile;
        if (right is { } r) tile.SetSize(right: r);
        tile.Position = tile.TowerCoordToNodePosition(x, y);
        //tile.OnMouseEntered += _ => _onMouseEnter(x, y);
        tile.OnAccept += (d) => onAccept(x, y, d);
        tile.OnCancel += _onCancel;
        return tile;
    }

    private void _onCancel(UserEvent d)
    {
        UIActions.Hide(new(this) { Source = d });
    }

    private void _onAcceptReplace(int x, int y, UserEvent d)
    {
        if (Tower.State.Floors.TryGetValue(y, out var floor) && _currentFloorDef != null)
        {
            FloorActions.Replace(new(Tower.State, floor, _currentFloorDef) { Source = d });
        }
    }

    private void _onAcceptExtend(int x, int y, UserEvent d)
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
            FloorActions.Extend(new(Tower.State, floor, left, right) { Source = d });
        }
    }

    private void _onAcceptNewBasement(int x, int y, UserEvent d)
    {
        FloorActions.Construct(new(Tower.State, Tower.State.NewBasementFloor(_currentFloorDef)) { Source = d });
    }

    private void _onAcceptNewTop(int x, int y, UserEvent d)
    {
        FloorActions.Construct(new(Tower.State, Tower.State.NewTopFloor(_currentFloorDef)) { Source = d });
    }

    private bool _canBuildFloorAt(int elevation) => _currentFloorDef?.CanBuildFloorAt(elevation) ?? false;
}
