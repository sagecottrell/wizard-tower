using Godot;
using System.Collections.Generic;
using wizardtower.events;
using wizardtower.state;

namespace wizardtower.containers;

public partial class RoomsContainerScript(TowerScript tower) : Node3D()
{
    public TowerScript Tower { get; } = tower;
    public TowerState State { get; } = tower.State;

    public Dictionary<RoomState, RoomScript> Rooms { get; } = [];

    public override void _Ready()
    {
        foreach (var room in State.Rooms.Values)
            SetupRoomDisplay(room);
        if (GlobalSignals.Singleton is GlobalSignals g)
        {
            g.OnRoomConstructing += _g_OnRoomConstructing;
            g.OnRoomConstructionStopping += _g_OnRoomConstructionStopping;
            g.OnRoomConstructed += _g_OnRoomConstructed;
            g.OnRoomDestroyed += _g_OnRoomDestroyed;
        }
    }

    private void _g_OnRoomDestroyed(RoomDestroyedEvent @event)
    {
        if (Rooms.Remove(@event.Room, out var node))
            node.QueueFree();
    }

    private void _g_OnRoomConstructed(RoomConstructedEvent @event)
    {
        SetupRoomDisplay(@event.Room);
    }

    public void SetupRoomDisplay(RoomState newRoom)
    {
        Rooms[newRoom] = this.AddedChild(new RoomScript() { State = newRoom  });
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
}
