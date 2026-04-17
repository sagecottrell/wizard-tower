using Godot;
using System.Collections.Generic;
using wizardtower.actions.ui;
using wizardtower.containers;
using wizardtower.events.ui;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;


public partial class TowerTransportBuilderOverlay(TowerScript tower) : Node3D()
{
    [Signal]
    public delegate void OnTransportBuildEventHandler();
    public TowerScript Tower { get; set; } = tower;
    public TransportScript? BuildingTransport { get; set; }
    private readonly Dictionary<(int elevation, int position), TransportSelected> _selected = [];

    private TransportDefinition? _currentTransportDef;

    private RichTextLabel _uiLabel = new()
    {
        BbcodeEnabled = true,
        FitContent = true,
        ClipContents = false,
        AutowrapMode = TextServer.AutowrapMode.Off,
        Visible = false,
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
        GlobalSignals.Singleton.OnTransportConstructionSelected += _onTransportConstructionSelected;
        GlobalSignals.Singleton.OnTransportConstructionStopped += _onTransportConstructionStopped;
        GlobalSignals.Singleton.OnCancelledUI += _onCancelledUI;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnTransportConstructionSelected -= _onTransportConstructionSelected;
        GlobalSignals.Singleton.OnTransportConstructionStopped -= _onTransportConstructionStopped;
        GlobalSignals.Singleton.OnCancelledUI -= _onCancelledUI;
    }

    private void _onTransportConstructionStopped(TransportConstructionStoppedEvent @event) => _reset();

    private void _onCancelledUI(CancelledUIEvent @event) => UIActions.BuildDeselectForce(Tower.State);

    private void _reset()
    {
        this.FreeChildren<TransportSelected>();
        _uiLabel.Visible = false;
        _selected.Clear();
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
        _currentTransportDef = @event.TransportDefinition;
        _uiLabel.Visible = true;
        _uiLabel.Text = $"Constructing: {_uiLabel.LineHeightImage(_currentTransportDef.Icon)} {_currentTransportDef.Name}";

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
                    _selected[(x, y)] = s;
                    s.Position = s.TowerCoordToNodePosition(x, y);
                    AddChild(s);
                    s.OnMouseEntered += _ => _onMouseEnter(x, y);
                    s.OnAccept += _ => _onAccept(x, y);
                    s.OnCancel += _ => _onCancel(x, y);
                }
            }
        }

    }

    private void _onMouseEnter(int x, int y)
    {
        _revertFloorVis();
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

    private void _onCancel(int x, int y)
    {
        UIActions.BuildDeselectForce(Tower.State);
    }

    private void _onAccept(int x, int y)
    {
        UIActions.BuildDeselectForce(Tower.State);
    }
}