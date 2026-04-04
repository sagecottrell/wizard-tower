using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using wizardtower.events;
using wizardtower.state;
using wizardtower.UIs.build_menu;

namespace wizardtower;

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

    public Dictionary<int, FloorScript> Floors { get; set; } = [];

    public override void _Ready()
    {
        State.EnsureGroundFloor();

        AddChild(new TowerBuilderOverlay(this));

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
                SetupRoomDisplay(room);
            }
        }
        BuildMenu?.SetTower(State);
        this.Child<UIManager>()?.ShowUI();

        if (GlobalSignals.Singleton is GlobalSignals g)
        {
            g.OnRoomConstructing += _g_OnRoomConstructing;
            g.OnRoomConstructed += _g_OnRoomConstructed;
            g.OnRoomConstructionStopping += _g_OnRoomConstructionStopping;
            g.OnFloorConstructed += _g_OnFloorConstructed;
        }
    }

    private void _g_OnRoomConstructionStopping(RoomConstructionStoppingEvent @event)
    {
        if (@event.TowerState != State)
            return;
        if (@event.RoomDefinition.CostToBuildPerUnit <= State.Wallet)
            @event.IsAllowed = false;
    }

    private void _g_OnRoomConstructing(RoomConstructingEvent @event)
    {
        if (@event.TowerState != State)
            return;
        if (@event.Room.Definition.CostToBuildPerUnit > State.Wallet)
        {
            this.Log("Not enough money to build this room.");
            @event.IsAllowed = false;
            return;
        }
        // enough money means it is allowed to build
    }

    private void _g_OnFloorConstructed(FloorConstructedEvent @event)
    {
        if (@event.TowerState != State)
            return;
        SetupFloorDisplay(@event.Floor);
    }

    private void _g_OnRoomConstructed(RoomConstructedEvent @event)
    {
        if (@event.TowerState != State)
            return;
        State.OnAddRoom(@event.Room);
        SetupRoomDisplay(@event.Room);

        var cost = @event.Room.Definition.CostToBuildPerUnit;
        if (GlobalSignals.TowerResourceChanging(new(State, cost)).IsAllowed)
        {
            State.Wallet.Subtracted(cost);
            GlobalSignals.TowerResourceChanged(new(State, cost));
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

    public void SetupFloorDisplay(FloorState newFloor)
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

    public void SetupRoomDisplay(RoomState newRoom)
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
                fs.SetPositionVisible(newRoom, false);
            }
        }
    }

}
