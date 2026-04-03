using Godot;
using System;
using System.Collections.Generic;
using wizardtower.events;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;

public partial class TowerBuilderOverlay(TowerScript tower) : Node3D()
{
    [Signal]
    public delegate void OnRoomConstructingEventHandler(RoomState room);

    public TowerScript? Tower { get; set; } = tower;

    public RoomScript? BuildingRoom { get; set; }
    private readonly Dictionary<(int elevation, int position), Selected> _selected = [];

    public override void _Ready()
    {
        if (GlobalSignals.Singleton is GlobalSignals g)
        {
            g.OnRoomConstructionSelected += _onRoomConstructionSelected;
            g.OnRoomConstructionStopped += _onRoomConstructionStopped;
        }
    }

    private void _onRoomConstructionStopped(RoomConstructionStoppedEvent @event)
    {
        this.FreeChildren<Selected>();
        _selected.Clear();
        _revertFloorVis();
        BuildingRoom?.QueueFree();
        BuildingRoom = null;
    }

    private void _onRoomConstructionSelected(RoomConstructionSelectedEvent @event)
    {
        if (@event.TowerState != Tower?.State || Tower?.State is null)
            return;

        foreach (var (h, floor) in Tower.State.Floors)
        {
            if (@event.RoomDefinition.AllowedFloors.Contains(floor.Definition))
            {
                for (var i = floor.LeftBound; i <= floor.RightBound; i++)
                {
                    if (Tower.State.PositionVacant(h, i) && SceneLoader.TryLoadScene<Selected>(out var s))
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
        if (@event.TowerState != Tower?.State || Tower?.State is null)
            return;
        if (GlobalSignals.RoomConstructionStopping(new(Tower.State, @event.RoomDefinition, userRequested: true)).IsAllowed)
            GlobalSignals.RoomConstructionStopped(new(Tower.State, @event.RoomDefinition));
    }

    private void _onAccept(RoomConstructionSelectedEvent @event, int x, int y)
    {
        if (@event.TowerState != Tower?.State || Tower?.State is null)
            return;
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
            EmitSignalOnRoomConstructing(room);
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
        if (@event.TowerState != Tower?.State || Tower?.State is null)
            return;
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
        Tower.Floors[y].SetPositionVisible(x, @event.RoomDefinition.Width, false);
    }

    private void _revertFloorVis()
    {
        if (BuildingRoom is not null && Tower is not null)
            Tower.Floors[BuildingRoom.State.Elevation].SetPositionVisible(BuildingRoom.State, true);
    }
}
