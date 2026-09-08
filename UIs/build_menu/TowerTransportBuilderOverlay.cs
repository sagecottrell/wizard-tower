using Godot;
using System;
using System.Collections.Generic;
using wizardtower.actions;
using wizardtower.actions.ui;
using wizardtower.containers;
using wizardtower.events.handlers;
using wizardtower.events.interfaces;
using wizardtower.events.Transport.ui;
using wizardtower.resource_types;
using wizardtower.state;
using wizardtower.UIs.selector;

namespace wizardtower.UIs.build_menu;


public partial class TowerTransportBuilderOverlay(TowerScript tower) : Node3D(), IUserInterface
{
    [Signal]
    public delegate void OnTransportBuildEventHandler();
    public TowerScript Tower { get; set; } = tower;
    public TransportScript? BuildingTransport { get; set; }
    private readonly Dictionary<(int elevation, int position), Selector> _positionSelected = [];
    private readonly Dictionary<(int elevation, int position), Selector> _heightSelected = [];

    private TransportDefinition? _currentTransportDef;

    private RichTextLabel _uiLabel = new()
    {
        BbcodeEnabled = true,
        FitContent = true,
        ClipContents = false,
        AutowrapMode = TextServer.AutowrapMode.Off,
        Visible = false,
    };

    private Node3D _positionSelectors = new();
    private Node3D _heightSelectors = new();

    public override void _Ready()
    {
        AddChild(new PanelContainer()
        {
            PivotOffsetRatio = new(0.5f, 0),
            AnchorLeft = 0.5f,
            AnchorRight = 0.5f,
            GrowHorizontal = Control.GrowDirection.Both,
        }.WithChild(_uiLabel));
        AddChild(_positionSelectors);
        AddChild(_heightSelectors);
    }

    public override void _EnterTree()
    {
        TransportEvents.Ui.ConstructionSelected += _onTransportConstructionSelected;
        TransportEvents.Ui.ConstructionStopped += _event_reset;
        FloorEvents.Ui.ConstructionSelected += _event_reset;
        RoomEvents.Ui.ConstructionSelected += _event_reset;
    }

    public override void _ExitTree()
    {
        TransportEvents.Ui.ConstructionSelected -= _onTransportConstructionSelected;
        TransportEvents.Ui.ConstructionStopped -= _event_reset;
        FloorEvents.Ui.ConstructionSelected -= _event_reset;
        RoomEvents.Ui.ConstructionSelected -= _event_reset;
    }

    private void _event_reset(IEvent @event) => _reset(@event);

    private void _reset(IEvent d)
    {
        _positionSelectors.FreeChildren<Selector>();
        _heightSelectors.FreeChildren<Selector>();
        _uiLabel.Visible = false;
        _positionSelected.Clear();
        _heightSelected.Clear();
        _revertFloorVis(d);
        _currentTransportDef = null;
        BuildingTransport?.QueueFree();
    }

    private void _revertFloorVis(IEvent d)
    {
        TransportEvents.Ui.OnConstructionPreviewStopped(new(Tower.State) { Source = d });
    }

    private void _onTransportConstructionSelected(TransportConstructionSelectedEvent @event)
    {
        if (_currentTransportDef == @event.TransportDefinition)
            return;
        _reset(@event);

        _currentTransportDef = @event.TransportDefinition;
        _uiLabel.Visible = true;
        _uiLabel.Text = $"Constructing: {_uiLabel.LineHeightImage(_currentTransportDef.Icon)} {_currentTransportDef.Name}";
        _positionSelectors.Visible = true;
        _heightSelectors.Visible = false;

        foreach (var floor in Tower.State.Floors.Values)
        {
            if (!_currentTransportDef.CanStopAtFloor.Contains(floor.Definition))
                continue;

            for (var i = floor.LeftBound; i <= floor.RightBound; i++)
            {
                if (Tower.State.PositionVacant(floor.Elevation, i) && SceneLoader.TryLoadScene<Selector>(out var s))
                {
                    var x = i;
                    var y = floor.Elevation;
                    _positionSelected[(x, y)] = s;
                    s.Position = s.TowerCoordToNodePosition(x, y);
                    _positionSelectors.AddChild(s);
                    s.OnMouseEntered += (d) => _onMouseEnterStart(x, y, d);
                    s.OnAccept += (d) => _onAcceptStart(x, y);
                    s.OnCancel += _onCancel;
                }
            }
        }
    }

    private void _onMouseEnterStart(int x, int y, UserEvent d)
    {
        _revertFloorVis(d);
        if (_currentTransportDef is null)
            return;
        BuildingTransport ??= this.AddedChild(new TransportScript(Tower)
        {
            HologramMode = true,
            State = new TransportState()
            {
                Definition = _currentTransportDef,
                Height = 1,
            }
        });
        BuildingTransport.State.Elevation = y;
        BuildingTransport.State.HorizontalPosition = x;
        TransportEvents.Ui.OnConstructionPreviewStarted(new(Tower.State, BuildingTransport.State) { Source = d });
    }

    private void _onCancel(UserEvent d)
    {
        UIActions.Hide(new(this) { Source = d });
    }

    private void _onAcceptStart(int floorPosition, int elevation)
    {
        if (_currentTransportDef is null)
            return;
        for (var e = elevation - (int)_currentTransportDef.MaxHeight + 1; e < elevation + _currentTransportDef.MaxHeight; e++)
        {
            if (!Tower.State.Floors.TryGetValue(e, out var floor))
                continue;
            if (!_currentTransportDef.CanStopAtFloor.Contains(floor.Definition))
                continue;

            if (Tower.State.PositionVacant(e, floorPosition) && SceneLoader.TryLoadScene<Selector>(out var s))
            {
                var x = floorPosition;
                var y = Math.Max(e, elevation);
                var y0 = Math.Min(e, elevation);
                var height = (uint)(y - y0) + 1;
                _heightSelected[(x, e)] = s;
                s.Position = s.TowerCoordToNodePosition(x, e);
                _heightSelectors.AddChild(s);
                s.OnMouseEntered += (d) => _onMouseEnterFinal(x, y0, height, d);
                s.OnAccept += (d) => _onAcceptFinal(x, y0, height, d);
                s.OnCancel += _onCancel;
            }
        }
        _positionSelectors.Visible = false;
        _heightSelectors.Visible = true;
    }

    private void _onMouseEnterFinal(int x, int y, uint height, UserEvent d)
    {
        _revertFloorVis(d);
        if (_currentTransportDef is null)
            return;
        BuildingTransport ??= this.AddedChild(new TransportScript(Tower)
        {
            HologramMode = true,
            State = new TransportState()
            {
                Definition = _currentTransportDef,
            }
        });
        BuildingTransport.State.Height = height;
        BuildingTransport.State.Elevation = y;
        BuildingTransport.State.HorizontalPosition = x;
        TransportEvents.Ui.OnConstructionPreviewStarted(new(Tower.State, BuildingTransport.State) { Source = d });
    }

    private void _onAcceptFinal(int x, int y, uint height, UserEvent d)
    {
        BuildingTransport?.QueueFree();
        BuildingTransport = null;
        if (_currentTransportDef is null)
            return;
        var room = new TransportState()
        {
            Definition = _currentTransportDef,
            Height = height,
            Elevation = y,
            HorizontalPosition = x,
        };
        TransportActions.Construct(new(Tower.State, room) { Source = d });

        if (TransportEvents.Ui.OnConstructionStopping(new(Tower.State, _currentTransportDef) { Source = d }).IsAllowed)
            TransportEvents.Ui.OnConstructionStopped(new(Tower.State, _currentTransportDef) { Source = d });
        else
        {
            for (var i = 0; i < _currentTransportDef.Width; i++)
            {
                if (_positionSelected.Remove((x + i, y), out var s))
                    s.QueueFree();
            }
        }
    }
}