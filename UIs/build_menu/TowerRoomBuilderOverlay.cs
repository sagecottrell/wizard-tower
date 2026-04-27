using Godot;
using System.Collections.Generic;
using wizardtower.actions;
using wizardtower.containers;
using wizardtower.events.interfaces;
using wizardtower.events.ui;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;

public partial class TowerRoomBuilderOverlay(TowerScript tower) : Node3D(), IUserInterface
{
    public TowerScript Tower { get; set; } = tower;

    public RoomScript? BuildingRoom { get; set; }
    private readonly Dictionary<(int elevation, int position), RoomSelected> _selected = [];

    private RoomDefinition? _currentRoomDef;

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
        GlobalSignals.Singleton.OnRoomConstructionSelected += _onRoomConstructionSelected;
        GlobalSignals.Singleton.OnRoomConstructionStopped += _event_reset;
        GlobalSignals.Singleton.OnFloorConstructionSelected += _event_reset;
        GlobalSignals.Singleton.OnTransportConstructionSelected += _event_reset;
    }
    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnRoomConstructionSelected -= _onRoomConstructionSelected;
        GlobalSignals.Singleton.OnRoomConstructionStopped -= _event_reset;
        GlobalSignals.Singleton.OnFloorConstructionSelected -= _event_reset;
        GlobalSignals.Singleton.OnTransportConstructionSelected -= _event_reset;
    }

    private void _reset()
    {
        this.FreeChildren<RoomSelected>();
        _uiLabel.Visible = false;
        _selected.Clear();
        _revertFloorVis();
        BuildingRoom?.QueueFree();
        _currentRoomDef = null;
        BuildingRoom = null;
    }

    private void _event_reset(IEvent @event) => _reset();

    private void _onRoomConstructionSelected(RoomConstructionSelectedEvent @event)
    {
        if (_currentRoomDef == @event.RoomDefinition)
            return;
        _reset();

        _currentRoomDef = @event.RoomDefinition;
        _uiLabel.Visible = true;
        _uiLabel.Text = $"Constructing: {_uiLabel.LineHeightImage(_currentRoomDef.Icon)} {_currentRoomDef.Name}";

        foreach (var (h, floor) in Tower.State.Floors)
        {
            if (@event.RoomDefinition.AllowedFloors.Contains(floor.Definition))
            {
                for (var i = floor.LeftBound; i <= floor.RightBound; i++)
                {
                    if (Tower.State.PositionVacant(h, i) && SceneLoader.TryLoadScene<RoomSelected>(out var s))
                    {
                        var x = i;
                        var y = h;
                        _selected[(x, y)] = s;
                        s.Position = s.TowerCoordToNodePosition(x, y);
                        AddChild(s);
                        s.OnMouseEntered += _ => _onMouseEnter(x, y);
                        s.OnAccept += _ => _onAccept(x, y);
                        s.OnCancel += _ => _onCancel();
                    }
                }
            }
        }
    }

    private void _onCancel()
    {
        if (_currentRoomDef != null)
            GlobalSignals.RoomConstructionStopped(new(Tower.State, _currentRoomDef));
    }

    private void _onAccept(int x, int y)
    {
        if (_currentRoomDef == null)
            return;
        BuildingRoom?.QueueFree();
        BuildingRoom = null;
        var room = new RoomState()
        {
            Definition = _currentRoomDef,
            Height = 1,
            Elevation = y,
            FloorPosition = x,
        };
        Actions.BuyRoom(new(Tower.State, room));

        if (GlobalSignals.RoomConstructionStopping(new(Tower.State, _currentRoomDef)).IsAllowed)
            GlobalSignals.RoomConstructionStopped(new(Tower.State, _currentRoomDef));
        else
        {
            for (var i = 0; i < _currentRoomDef.Width; i++)
            {
                if (_selected.Remove((x + i, y), out var s))
                    s.QueueFree();
            }
        }
    }

    private void _onMouseEnter(int x, int y)
    {
        if (_currentRoomDef == null)
            return;
        _revertFloorVis();
        BuildingRoom ??= this.AddedChild(new RoomScript()
        {
            HologramMode = true,
            State = new RoomState()
            {
                Definition = _currentRoomDef,
                Height = 1,
            }
        });
        BuildingRoom.State.Elevation = y;
        BuildingRoom.State.FloorPosition = x;
        GlobalSignals.RoomConstructionPreview(new(Tower.State, BuildingRoom.State));
    }

    private void _revertFloorVis()
    {
        GlobalSignals.RoomConstructionPreviewStopped(new(Tower.State));
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event.IsActionPressed(InputMapConstants.Cancel))
        {
            _onCancel();
            GetViewport().SetInputAsHandled();
        }
    }
}
