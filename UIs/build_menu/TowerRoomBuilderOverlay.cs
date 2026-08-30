using Godot;
using System.Collections.Generic;
using wizardtower.actions;
using wizardtower.containers;
using wizardtower.events.handlers;
using wizardtower.events.interfaces;
using wizardtower.events.Room.ui;
using wizardtower.resource_types;
using wizardtower.state;
using wizardtower.UIs.selector;

namespace wizardtower.UIs.build_menu;

public partial class TowerRoomBuilderOverlay(TowerScript tower) : Node3D(), IUserInterface
{
    public TowerScript Tower { get; set; } = tower;

    public RoomScript? BuildingRoom { get; set; }
    private readonly Dictionary<(int elevation, int position), Selector> _selected = [];

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
        RoomEvents.Ui.ConstructionSelected += _onRoomConstructionSelected;
        RoomEvents.Ui.ConstructionStopped += _event_reset;
        FloorEvents.Ui.ConstructionSelected += _event_reset;
        TransportEvents.Ui.ConstructionSelected += _event_reset;
    }
    public override void _ExitTree()
    {
        RoomEvents.Ui.ConstructionSelected -= _onRoomConstructionSelected;
        RoomEvents.Ui.ConstructionStopped -= _event_reset;
        FloorEvents.Ui.ConstructionSelected -= _event_reset;
        TransportEvents.Ui.ConstructionSelected -= _event_reset;
    }

    private void _reset(IEvent d)
    {
        this.FreeChildren<Selector>();
        _uiLabel.Visible = false;
        _selected.Clear();
        _revertFloorVis(d);
        BuildingRoom?.QueueFree();
        _currentRoomDef = null;
        BuildingRoom = null;
    }

    private void _event_reset(IEvent @event) => _reset(@event);

    private void _onRoomConstructionSelected(RoomConstructionSelectedEvent @event)
    {
        if (_currentRoomDef == @event.RoomDefinition)
            return;
        _reset(@event);

        _currentRoomDef = @event.RoomDefinition;
        _uiLabel.Visible = true;
        _uiLabel.Text = $"Constructing: {_uiLabel.LineHeightImage(_currentRoomDef.Icon)} {_currentRoomDef.Name}";

        foreach (var (h, floor) in Tower.State.Floors)
        {
            if (@event.RoomDefinition.AllowedFloors.Contains(floor.Definition))
            {
                for (var i = floor.LeftBound; i <= floor.RightBound; i++)
                {
                    if (Tower.State.PositionVacant(h, i, _currentRoomDef.Width, _currentRoomDef.Height) && SceneLoader.TryLoadScene<Selector>(out var s))
                    {
                        var x = i;
                        var y = h;
                        _selected[(x, y)] = s;
                        s.Position = s.TowerCoordToNodePosition(x, y);
                        AddChild(s);
                        s.OnMouseEntered += (d) => _onMouseEnter(x, y, d);
                        s.OnAccept += (d) => _onAccept(x, y, d);
                        s.OnCancel += _onCancel;
                    }
                }
            }
        }
    }

    private void _onCancel(UserEvent d)
    {
        if (_currentRoomDef != null)
            RoomEvents.Ui.OnConstructionStopped(new(Tower.State, _currentRoomDef) { Source = d });
    }

    private void _onAccept(int x, int y, UserEvent d)
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
        RoomActions.Construct(new(Tower.State, room) { Source = d });

        if (RoomEvents.Ui.OnConstructionStopping(new(Tower.State, _currentRoomDef) { Source = d }).IsAllowed)
            RoomEvents.Ui.OnConstructionStopped(new(Tower.State, _currentRoomDef) { Source = d });
        else
        {
            for (var i = 0; i < _currentRoomDef.Width; i++)
            {
                for (var j = 0; j < _currentRoomDef.Height; j++)
                {
                    if (_selected.Remove((x + i, y + j), out var s))
                        s.QueueFree();
                    if ((j != 0 || i != 0) && _selected.Remove((x - i, y - j), out var s2))
                        s2.QueueFree();
                }
            }
        }
    }

    private void _onMouseEnter(int x, int y, UserEvent d)
    {
        if (_currentRoomDef == null)
            return;
        _revertFloorVis(d);
        BuildingRoom ??= this.AddedChild(new RoomScript(Tower)
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
        RoomEvents.Ui.OnConstructionPreviewStarted(new(Tower.State, BuildingRoom.State) { Source = d });
    }

    private void _revertFloorVis(IEvent d)
    {
        RoomEvents.Ui.OnConstructionPreviewStopped(new(Tower.State) { Source = d });
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event.IsActionPressed(InputMapConstants.Cancel))
        {
            _onCancel(@event);
            GetViewport().SetInputAsHandled();
        }
    }
}
