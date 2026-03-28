using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using wizardtower.resource_types;
using wizardtower.state;
using wizardtower.UIs.build_menu;

namespace wizardtower;

[Tool]
[GlobalClass]
public partial class TowerScript : Node3D
{
    [Export]
    public TowerState State { get; set; } = new();

    private TowerState PreviousState { get; set; } = new();

    [Export]
    public Node3D? FloorsContainer { get; set; }

    [Export]
    public Node3D? WorkersContainer { get; set; }

    [Export]
    public Node3D? RoofsContainer { get; set; }

    [Export]
    public Node3D? Camera { get; set; }

    [Export]
    public BuildMenu? BuildMenu { get; set; }

    public Resource? WhatAreWeBuilding { get; set; }

    public Dictionary<int, FloorScript> Floors { get; set; } = [];

    public override void _Ready()
    {
        State.EnsureGroundFloor();

        if (FloorsContainer is not null)
        {
            foreach (var newFloor in State.Floors.Keys)
            {
                var fs = new FloorScript() { State = State.Floors[newFloor] };
                FloorsContainer.AddChild(fs);
                Floors[newFloor] = fs;
                fs.SetupTiles();
            }
            foreach (var (roomId, room) in State.Rooms)
            {
                OnRoomAdd(room);
            }
        }
        BuildMenu?.SetTower(State);
        this.Child<UIManager>()?.ShowUI();

        if (GlobalSignals.Singleton is GlobalSignals g)
        {
            g.OnStartedRoomConstruction += _g_OnStartedRoomConstruction;
            g.OnRoomConstructed += _g_OnRoomConstructed;
            g.OnFloorConstructed += _g_OnFloorConstructed;
        }
    }

    private void _g_OnFloorConstructed(events.FloorConstructedEvent @event)
    {
        if (@event.TowerState != State)
            return;
        OnFloorAdd(@event.Floor);
    }

    private void _g_OnRoomConstructed(events.RoomConstructedEvent @event)
    {
        if (@event.TowerState != State)
            return;
        OnRoomAdd(@event.Room);
    }

    private void _g_OnStartedRoomConstruction(events.StartedRoomConstructionEvent @event)
    {
        if (@event.TowerState != State)
            return;
        WhatAreWeBuilding = @event.RoomDefinition;
        BuildMenu?.SetWhatAreWeBuilding(WhatAreWeBuilding);

        if (WhatAreWeBuilding is RoomDefinition r && FloorsContainer is not null)
        {
            foreach (var (h, floor) in State.Floors)
            {
                if (r.AllowedFloors.Contains(floor.Definition))
                {
                    for (var i = floor.LeftBound; i <= floor.RightBound; i++)
                    {
                        if (State.PositionVacant(h, i) && SceneLoader.TryLoadScene<Selected>(out var s))
                        {
                            s.Position = s.TowerCoordToNodePosition(x: i, y: h);
                            FloorsContainer.AddChild(s);
                        }
                    }
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        if (State != PreviousState && State.Compare(PreviousState))
            return;

        if (State.Floors.Count < PreviousState.Floors.Count && FloorsContainer is not null)
        {
            var removedFloors = PreviousState.Floors.Keys.Except(State.Floors.Keys).ToHashSet();
            foreach (var child in FloorsContainer.GetChildren())
            {
                if (child is FloorScript fs && removedFloors.Contains(fs.State.Elevation))
                    fs.Destroy();
            }
        }

        if (WorkersContainer is not null)
        {
            WorkersContainer.Position = WorkersContainer.Position with { Z = Math.Max(2, State.MaxBasement) + 2 };
        }

        if (Camera is not null)
        {
            Camera.Position = Camera.Position with { Z = Math.Max(2, State.MaxBasement) + 3 };
        }

        PreviousState = State.Copy();
    }

    public void OnFloorAdd(FloorState newFloor)
    {
        if (FloorsContainer is null)
            return;
        var fs = new FloorScript()
        {
            State = newFloor,
        };
        fs.OnCreate();
        Floors[newFloor.Elevation] = fs;
        fs.SetupTiles();
        FloorsContainer.AddChild(fs);
    }

    public void OnRoomAdd(RoomState newRoom)
    {
        if (FloorsContainer is null)
            return;
        var room = new RoomScript()
        {
            State = newRoom,
        };
        FloorsContainer.AddChild(room);

        for (var i = 0; i < newRoom.Height; i++)
        {
            if (Floors.TryGetValue(newRoom.Elevation + i, out var fs))
            {
                fs.SetPositionVisible(newRoom.FloorPosition, newRoom.Definition.Width, false);
            }
        }
    }

}
