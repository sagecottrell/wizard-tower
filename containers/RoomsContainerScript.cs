using Godot;
using System.Collections.Generic;
using System.Linq;
using wizardtower.actions.ui;
using wizardtower.events.handlers;
using wizardtower.events.Room;
using wizardtower.events.Room.ui;
using wizardtower.state;

namespace wizardtower.containers;

public partial class RoomsContainerScript(TowerScript tower) : Node3D()
{
    public TowerScript Tower { get; } = tower;
    public TowerState State { get; } = tower.State;

    public int RoomsPerCycle = 4;
    public int RoomCycle = 0;

    public Dictionary<RoomState, RoomScript> Rooms { get; } = [];

    public override void _Ready()
    {
        Name = nameof(RoomsContainerScript);
        foreach (var room in State.Rooms.Values)
            SetupRoomDisplay(room);
    }

    public override void _EnterTree()
    {
        RoomEvents.Constructing += _onRoomConstructing;
        RoomEvents.UI.ConstructionStopping += _onRoomConstructionStopping;
        RoomEvents.Constructed += _onRoomConstructed;
        RoomEvents.Destroyed += _onRoomDestroyed;
    }

    public override void _ExitTree()
    {
        RoomEvents.Constructing -= _onRoomConstructing;
        RoomEvents.UI.ConstructionStopping -= _onRoomConstructionStopping;
        RoomEvents.Constructed -= _onRoomConstructed;
        RoomEvents.Destroyed -= _onRoomDestroyed;
    }

    private void _onRoomDestroyed(RoomDestroyedEvent @event)
    {
        if (Rooms.Remove(@event.Room, out var node))
            node.QueueFree();
    }

    private void _onRoomConstructed(RoomConstructedEvent @event)
    {
        SetupRoomDisplay(@event.Room);
    }

    public void SetupRoomDisplay(RoomState newRoom)
    {
        Rooms[newRoom] = this.AddedChild(new RoomScript(Tower) { State = newRoom });
    }

    private void _onRoomConstructionStopping(RoomConstructionStoppingEvent @event)
    {
        if (@event.TowerState != State)
            return;
        if (@event.RoomDefinition.CostToBuildPerUnit <= State.Wallet) 
            @event.IsAllowed = false;
    }

    private void _onRoomConstructing(RoomConstructingEvent @event)
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

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton ms && ms.IsActionPressed(InputMapConstants.LeftClick) && GetViewport().GetCamera3D() is Camera3D camera)
        {
            var dir = camera.ProjectRayOrigin(ms.Position);
            var x = Mathf.FloorToInt(dir.X + 0.5f);
            var y = Mathf.FloorToInt(dir.Y);
            if (Tower.State.RoomsOnFloor(y).FirstOrDefault(r => x >= r.FloorPosition && x < r.FloorPosition + r.Definition.Width) is RoomState room)
            {
                UIActions.SelectRoom(new(Tower.State, room) { Input = @event });
            }
        }
    }

    public override void _Process(double delta)
    {
        var d = delta * RoomsPerCycle;
        var simultaneous = Rooms.Count / RoomsPerCycle;
        var rooms = Rooms.Values.ToList();
        RoomCycle = (RoomCycle + 1) % RoomsPerCycle;
        for (int i = 0; i < simultaneous; i++)
        {
            var id = i * RoomsPerCycle + RoomCycle;
            if (id >= rooms.Count)
                break;
            var room = rooms[id];
            room.ProcessRoomFunctions(d);
        }
    }
}
