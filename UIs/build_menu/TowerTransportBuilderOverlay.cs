using Godot;
using System;
using System.Collections.Generic;
using wizardtower.actions;
using wizardtower.actions.ui;
using wizardtower.containers;
using wizardtower.events.interfaces;
using wizardtower.events.ui;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;


public partial class TowerTransportBuilderOverlay(TowerScript tower) : Node3D(), IUserInterface
{
    [Signal]
    public delegate void OnTransportBuildEventHandler();
    public TowerScript Tower { get; set; } = tower;
    public TransportScript? BuildingTransport { get; set; }
    private readonly Dictionary<(int elevation, int position), TransportSelected> _positionSelected = [];
    private readonly Dictionary<(int elevation, int position), TransportSelected> _heightSelected = [];

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
        GlobalSignals.Singleton.OnTransportConstructionSelected += _onTransportConstructionSelected;
        GlobalSignals.Singleton.OnTransportConstructionStopped += _event_reset;
        GlobalSignals.Singleton.OnFloorConstructionSelected += _event_reset;
        GlobalSignals.Singleton.OnRoomConstructionSelected += _event_reset;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnTransportConstructionSelected -= _onTransportConstructionSelected;
        GlobalSignals.Singleton.OnTransportConstructionStopped -= _event_reset;
        GlobalSignals.Singleton.OnFloorConstructionSelected -= _event_reset;
        GlobalSignals.Singleton.OnRoomConstructionSelected -= _event_reset;
    }

    private void _event_reset(IEvent @event) => _reset();

    private void _reset()
    {
        _positionSelectors.FreeChildren<TransportSelected>();
        _heightSelectors.FreeChildren<TransportSelected>();
        _uiLabel.Visible = false;
        _positionSelected.Clear();
        _heightSelected.Clear();
        _revertFloorVis();
        _currentTransportDef = null;
        BuildingTransport?.QueueFree();
    }

    private void _revertFloorVis()
    {
        GlobalSignals.TransportConstructionPreviewStopped(new(Tower.State));
    }

    private void _onTransportConstructionSelected(TransportConstructionSelectedEvent @event)
    {
        if (_currentTransportDef == @event.TransportDefinition)
            return;
        _reset();

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
                if (Tower.State.PositionVacant(floor.Elevation, i) && SceneLoader.TryLoadScene<TransportSelected>(out var s))
                {
                    var x = i;
                    var y = floor.Elevation;
                    _positionSelected[(x, y)] = s;
                    s.Position = s.TowerCoordToNodePosition(x, y);
                    _positionSelectors.AddChild(s);
                    s.OnMouseEntered += _ => _onMouseEnterStart(x, y);
                    s.OnAccept += _ => _onAcceptStart(x, y);
                    s.OnCancel += _ => _onCancel();
                }
            }
        }

    }

    private void _onMouseEnterStart(int x, int y)
    {
        _revertFloorVis();
        if (_currentTransportDef is null)
            return;
        BuildingTransport ??= this.AddedChild(new TransportScript()
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
        GlobalSignals.TransportConstructionPreview(new(Tower.State, BuildingTransport.State));
    }

    private void _onCancel()
    {
        UIActions.Hide(new(this));
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

            if (Tower.State.PositionVacant(e, floorPosition) && SceneLoader.TryLoadScene<TransportSelected>(out var s))
            {
                var x = floorPosition;
                var y = Math.Max(e, elevation);
                var y0 = Math.Min(e, elevation);
                var height = (uint)(y - y0) + 1;
                _heightSelected[(x, e)] = s;
                s.Position = s.TowerCoordToNodePosition(x, e);
                _heightSelectors.AddChild(s);
                s.OnMouseEntered += _ => _onMouseEnterFinal(x, y0, height);
                s.OnAccept += _ => _onAcceptFinal(x, y0, height);
                s.OnCancel += _ => _onCancel();
            }
        }
        _positionSelectors.Visible = false;
        _heightSelectors.Visible = true;
    }

    private void _onMouseEnterFinal(int x, int y, uint height)
    {
        _revertFloorVis();
        if (_currentTransportDef is null)
            return;
        BuildingTransport ??= this.AddedChild(new TransportScript()
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
        GlobalSignals.TransportConstructionPreview(new(Tower.State, BuildingTransport.State));
    }

    private void _onAcceptFinal(int x, int y, uint height)
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
        Actions.BuyTransport(new(Tower.State, room));

        if (GlobalSignals.TransportConstructionStopping(new(Tower.State, _currentTransportDef)).IsAllowed)
            GlobalSignals.TransportConstructionStopped(new(Tower.State, _currentTransportDef));
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