using Godot;
using MEC;
using System.Collections.Generic;
using System.Linq;
using wizardtower.actions;
using wizardtower.events.handlers;
using wizardtower.events.Room;
using wizardtower.events.Room.ui;
using wizardtower.resource_types;
using wizardtower.resource_types.room_functions;
using wizardtower.state;
using wizardtower.state.room_functions;
using wizardtower.UIs.room_details;

namespace wizardtower.containers;

[Tool]
[GlobalClass]
public partial class RoomScript(TowerScript tower) : Node3D
{
    public RoomState State { get; set; } = new();

    public RoomState PreviousState { get; set; } = new();

    private RoomBackgroundScript? RoomScene { get; set; }

    public bool HologramMode { get; set; } = false;
    public TowerScript Tower { get; } = tower;

    public override void _Ready()
    {
        Name = $"Room{State.Id}";
        if (HologramMode)
            AsHologram();
        else
            AsBackground();
    }

    public override void _EnterTree()
    {
        RoomEvents.UI.Selected += _onRoomSelected;
        RoomEvents.UI.Deselected += _onRoomDeselected;

        RoomEvents.ProducedResources += _onProducedResources;
        RoomEvents.ConsumedResources += _onConsumedResources;
    }

    public override void _ExitTree()
    {
        RoomEvents.UI.Selected -= _onRoomSelected;
        RoomEvents.UI.Deselected -= _onRoomDeselected;

        RoomEvents.ProducedResources -= _onProducedResources;
        RoomEvents.ConsumedResources -= _onConsumedResources;
    }

    private void _onRoomSelected(RoomSelectedEvent @event)
    {
        if (@event.RoomState.Id != State.Id) return;
        if (State.WorkerPaths is null || State.WorkerPaths.Count == 0) return;

        var offset = 0f;
        foreach (var workerPath in State.WorkerPaths)
        {
            var vis = new ResourceDeliveryVisualizer()
            {
                WorkerPath = workerPath,
                FromRoomId = State.Id,
                TowerState = @event.TowerState,
                Speed = 0.5f,
                ItemDistance = 2f,
                ItemScale = new(0.5f, 0.5f),
                Easing = 0.147f,
                TimeOffset = offset,
            };
            offset += 1f;
            vis.SetupPath();
            vis.Position = new(0, 0.5f, 2);
            AddChild(vis);
        }
    }

    private void _onRoomDeselected(RoomDeselectedEvent @event)
    {
        if (@event.RoomState.Id != State.Id) return;
        this.FreeChildren<ResourceDeliveryVisualizer>();
    }

    public override void _Process(double delta)
    {
        if (State != PreviousState && State.Compare(PreviousState))
            return;

        if (PreviousState.Definition.RoomScene != State.Definition.RoomScene)
        {
            RoomScene?.QueueFree();
            if (State.Definition.RoomScene is PackedScene scene)
            {
                RoomScene = scene.Instantiate() as RoomBackgroundScript;
                AddChild(RoomScene);
            }
        }

        if (HologramMode)
            AsHologram();
        else
            AsBackground();

        Position = this.TowerCoordToNodePosition(x: State.FloorPosition, y: State.Elevation);

        PreviousState = State.Copy();
    }

    public RoomScript AsHologram()
    {
        HologramMode = true;
        RoomScene?.AsHologram();
        return this;
    }

    public RoomScript AsBackground()
    {
        HologramMode = false;
        RoomScene?.AsBackground();
        return this;
    }

    private void _onProducedResources(RoomProducedResourcesEvent ev)
    {
        if (State != ev.RoomState || ev.Output is null)
            return;
        // this room just produced resources, let's see if we need to deliver them anywhere
        var destinations = State.WorkerPaths.Where(wp => ev.Output.ContainsKey(wp.ItemDefinition)).ToList();
        if (destinations.Count > 0)
        {
            // this implements a priority approach to choosing destinations. those earlier in the list have priority for delieveries.
            foreach (var dest in destinations)
                _tryDistributeResources(dest);
        }
    }

    private IEnumerator<double> _onConsumedResources(RoomConsumedResourcesEvent ev)
    {
        if (ev.RoomState != State)
        {
            // another room just consumed resources, let's see if it's a room we deliver to so that we can resupply it
            if (State.WorkerPaths.FirstOrDefault(wp => wp.TargetRoomId == ev.RoomState.Id && ev.Amount.ContainsKey(wp.ItemDefinition)) is RoomStateWorkerPath dest)
            {
                yield return Timing.WaitForSeconds(0.5);
                _tryDistributeResources(dest);
            }
        }
    }

    public void ProcessRoomFunctions(double delta)
    {
        foreach (var pair in State.Definition.RoomFunctions.Zip(State.FunctionStates))
        {
            switch (pair)
            {
                case (RoomConvertResourcesDefinition convertDef, RoomConvertResourcesState convertState):
                    {
                        if (convertDef.MaxTimesPerDay > 0 && convertState.TimesProducedToday >= convertDef.MaxTimesPerDay)
                            break;

                        if (convertState.CurrentlyWorking)
                        {
                            if (convertDef.ProcessingTimeSeconds > 0)
                            {
                                convertState.ProductionProgress += delta;
                                if (convertState.ProductionProgress > convertDef.ProcessingTimeSeconds)
                                {
                                    RoomActions.ProduceResources(new(Tower.State, State, convertDef, convertState));
                                }
                            }
                            else
                            {
                                RoomActions.ProduceResources(new(Tower.State, State, convertDef, convertState));
                            }
                        }
                        else
                        {
                            if (State.StoredItems >= convertDef.Recipe.Input)
                            {
                                RoomActions.ConsumeResources(new(Tower.State, State, convertDef.Recipe.Input));
                                RoomActions.StartWork(new(Tower.State, State, convertDef, convertState));
                            }
                        }

                        break;
                    }
            }
        }
    }

    private void _tryDistributeResources(RoomStateWorkerPath wp)
    {
        var targetRoom = Tower.State.Rooms[wp.TargetRoomId];
        var avgTime = wp.TimeTakenRecords.Average();

        foreach (var def in targetRoom.Definition.RoomFunctions)
        {
            switch (def)
            {
                case RoomConvertResourcesDefinition convertDef:
                    {
                        var avgReq = (uint)avgTime / convertDef.ProcessingTimeSeconds * convertDef.Recipe.Input;
                        var onTheWay = new NumericDict<ItemDefinition, uint>();
                        var stillRequired = avgReq - onTheWay;
                        foreach (var (item, requiredAmount) in stillRequired)
                        {
                            if (requiredAmount > 0 && State.StoredItems.GetOrDefault(item) > 0)
                            {
                                var amount = System.Math.Min(requiredAmount, State.StoredItems.GetOrDefault(item));

                                RoomActions.ConsumeResources(new(Tower.State, State, new() { [item] = amount }));
                                RoomActions.SpawnWorkerWithPayload(new(Tower.State, State, targetRoom, item, amount, convertDef.WorkerKind));
                            }
                        }
                        break;
                    }
            }
        }
    }
}
