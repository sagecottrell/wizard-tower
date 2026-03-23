using Godot;
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

    public override void _Process(double delta)
    {
        if (State == PreviousState)
            return;

        if (State.Floors.Count > PreviousState.Floors.Count)
        {
            var newFloors = State.Floors.Keys.Except(PreviousState.Floors.Keys);
            var floorsContainer = GetNode<Node3D>("Floors");
            foreach (var newFloor in newFloors) 
            {
                floorsContainer.AddChild(new FloorScript() {
                    State = State.Floors[newFloor],
                });
            }
        }

        PreviousState = State.Copy();
    }
}
