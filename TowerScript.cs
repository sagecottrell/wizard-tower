using Godot;
using PhantomCamera;
using System;
using System.Linq;
using wizardtower.state;

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
    public Node3D? RoomsContainer { get; set; }

    [Export]
    public Node3D? WorkersContainer { get; set; }

    [Export]
    public Node3D? TransportsContainer { get; set; }

    [Export]
    public Node3D? RoofsContainer { get; set; }

    [Export]
    public Node3D? Camera { get; set; }

    public override void _Ready()
    {
        State.EnsureGroundFloor();

        if (FloorsContainer is not null)
            foreach (var newFloor in State.Floors.Keys)
            {
                FloorsContainer.AddChild(new FloorScript()
                {
                    State = State.Floors[newFloor],
                });
            }
        State.OnFloorAdd += OnFloorAdd;
    }

    public override void _Process(double delta)
    {
        if (ReferenceEquals(State, PreviousState) == false && State == PreviousState)
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

        if (TransportsContainer is not null)
        {
            TransportsContainer.Position = TransportsContainer.Position with { Z = Math.Max(2, State.MaxBasement) + 1 };
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
        FloorsContainer.AddChild(fs);
    }
}
