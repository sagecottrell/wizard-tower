using Godot;
using System.Collections.Generic;
using wizardtower.containers;
using wizardtower.events;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;

public partial class TowerRoomBuilderOverlay(TowerScript tower) : Node3D()
{
    [Signal]
    public delegate void OnRoomConstructEventHandler(RoomConstructedEvent @event);

    public TowerScript Tower { get; set; } = tower;

    public RoomScript? BuildingRoom { get; set; }
    private readonly Dictionary<(int elevation, int position), RoomSelected> _selected = [];

    private RoomDefinition? _currentRoomDef;

    public override void _Ready()
    {
        if (GlobalSignals.Singleton is GlobalSignals g)
        {
            g.OnRoomConstructionSelected += _onRoomConstructionSelected;
            g.OnRoomConstructionStopped += _onRoomConstructionStopped;
        }
    }

    private void _reset()
    {
        this.FreeChildren<RoomSelected>();
        _selected.Clear();
        _revertFloorVis();
        BuildingRoom?.QueueFree();
        _currentRoomDef = null;
        BuildingRoom = null;
    }

    private void _onRoomConstructionStopped(RoomConstructionStoppedEvent @event) => _reset();

    private void _onRoomConstructionSelected(RoomConstructionSelectedEvent @event)
    {
        if (@event.TowerState != Tower.State)
            return;

        if (_currentRoomDef is not null)
        {
            GlobalSignals.RoomConstructionStopped(new(Tower.State, _currentRoomDef));
            return;
        }

        _currentRoomDef = @event.RoomDefinition;

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
                        s.OnMouseEntered += _ => _onMouseEnter(@event, x, y);
                        s.OnAccept += _ => _onAccept(@event, x, y);
                        s.OnCancel += _ => _onCancel(@event, x, y);
                    }
                }
            }
        }
    }

    private void _onCancel(RoomConstructionSelectedEvent @event, int x, int y)
    {
        // the user tried to cancel the construction of a room
        GlobalSignals.RoomConstructionStopped(new(Tower.State, @event.RoomDefinition));
    }

    private void _onAccept(RoomConstructionSelectedEvent @event, int x, int y)
    {
        BuildingRoom?.QueueFree();
        BuildingRoom = null;
        var room = new RoomState()
        {
            Definition = @event.RoomDefinition,
            Height = 1,
            Elevation = y,
            FloorPosition = x,
        };
        if (GlobalSignals.RoomConstructing(new(Tower.State, room)).IsAllowed)
        {
            EmitSignalOnRoomConstruct(new(Tower.State, room));
            GlobalSignals.RoomConstructed(new(Tower.State, room));

            if (GlobalSignals.RoomConstructionStopping(new(Tower.State, @event.RoomDefinition)).IsAllowed)
                GlobalSignals.RoomConstructionStopped(new(Tower.State, @event.RoomDefinition));
            else
            {
                for (var i = 0; i < @event.RoomDefinition.Width; i++)
                {
                    if (_selected.Remove((x + i, y), out var s))
                        s.QueueFree();
                }
            }
        }
    }

    private void _onMouseEnter(RoomConstructionSelectedEvent @event, int x, int y)
    {
        _revertFloorVis();
        BuildingRoom ??= this.AddedChild(new RoomScript()
        {
            HologramMode = true,
            State = new RoomState()
            {
                Definition = @event.RoomDefinition,
                Height = 1,
            }
        });
        BuildingRoom.State.Elevation = y;
        BuildingRoom.State.FloorPosition = x;
        GlobalSignals.RoomConstructionPreview(new(Tower.State, BuildingRoom.State));
    }

    private void _revertFloorVis()
    {
        GlobalSignals.RoomConstructionPreview(new(Tower.State, null));
    }
}
