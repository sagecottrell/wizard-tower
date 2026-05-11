using Godot;
using System.Linq;
using wizardtower.actions;
using wizardtower.events.handlers;
using wizardtower.events.Room.ui;
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
        RoomEvents.UI.RoomSelected += _onRoomSelected;
        RoomEvents.UI.RoomDeselected += _onRoomDeselected;
    }

    public override void _ExitTree()
    {
        RoomEvents.UI.RoomSelected -= _onRoomSelected;
        RoomEvents.UI.RoomDeselected -= _onRoomDeselected;
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

    public void ProcessRoomFunctions(double delta)
    {
        foreach (var (def, state) in State.Definition.RoomFunctions.Zip(State.FunctionStates))
        {
            switch ((def, state))
            {
                case (RoomConvertResourcesDefinition convertDef, RoomConvertResourcesState convertState):
                    {
                        if (convertDef.MaxTimesPerDay > 0 && convertState.TimesProducedToday >= convertDef.MaxTimesPerDay)
                            break;
                        if (convertDef.ProcessingTimeSeconds > 0)
                        {
                            convertState.ProductionProgress += delta;
                            if (convertState.ProductionProgress > convertDef.ProcessingTimeSeconds)
                            {
                                Actions.RoomProduceResources(new(Tower.State, State, convertDef, convertState));
                            }
                        }

                        break;
                    }
            }
        }
    }
}
