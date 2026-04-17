using Godot;
using System.Collections.Generic;
using wizardtower.events;
using wizardtower.events.ui;
using wizardtower.state;

namespace wizardtower.containers;

public partial class FloorsContainerScript(TowerScript tower) : Node3D()
{
    public TowerScript Tower { get; } = tower;
    public TowerState State { get; } = tower.State;

    public Dictionary<int, FloorScript> Floors { get; set; } = [];

    public override void _Ready()
    {
        foreach (var floor in State.Floors.Values)
            SetupFloorDisplay(floor);
    }

    public override void _EnterTree()
    {
        GlobalSignals.Singleton.OnFloorConstructing += _g_OnFloorConstructing;
        GlobalSignals.Singleton.OnFloorExtending += _g_OnFloorExtending;
        GlobalSignals.Singleton.OnFloorConstructionStopping += _g_OnFloorConstructionStopping;
        GlobalSignals.Singleton.OnFloorConstructed += _g_OnFloorConstructed;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnFloorConstructing -= _g_OnFloorConstructing;
        GlobalSignals.Singleton.OnFloorExtending -= _g_OnFloorExtending;
        GlobalSignals.Singleton.OnFloorConstructionStopping -= _g_OnFloorConstructionStopping;
        GlobalSignals.Singleton.OnFloorConstructed -= _g_OnFloorConstructed;
    }

    private void _g_OnFloorConstructed(FloorConstructedEvent @event)
    {
        SetupFloorDisplay(@event.Floor);
    }

    private void _state_OnFloorRemoved(FloorState floor)
    {
        if (Floors.Remove(floor.Elevation, out var fs))
            fs.Destroy();
    }

    public void SetupFloorDisplay(FloorState newFloor)
    {
        var fs = new FloorScript(State, newFloor);
        fs.OnCreate();
        fs.SetupTiles();
        Floors[newFloor.Elevation] = fs;
        AddChild(fs);
    }

    private void _g_OnFloorExtending(FloorExtendingEvent @event)
    {
        if (@event.Floor.Definition.CostToBuildPerUnit * @event.ExtensionAmount > State.Wallet)
        {
            this.Log("Not enough money to build this room.");
            @event.IsAllowed = false;
            return;
        }
        // enough money means it is allowed to build
    }

    private void _g_OnFloorConstructing(FloorConstructingEvent @event)
    {
        if (@event.Floor.Definition.CostToBuildPerUnit * @event.Floor.Width > State.Wallet)
        {
            this.Log("Not enough money to build this room.");
            @event.IsAllowed = false;
            return;
        }
        // enough money means it is allowed to build
    }

    private void _g_OnFloorConstructionStopping(FloorConstructionStoppingEvent @event)
    {
        if (@event.FloorDefinition.CostToBuildPerUnit <= State.Wallet)
            @event.IsAllowed = false;
    }
}
