using Godot;
using wizardtower.state;

namespace wizardtower.containers;

public partial class WorkerScript(TowerState towerState, WorkerState worker) : Node3D
{
    public TowerState TowerState { get; } = towerState;
    public WorkerState Worker { get; } = worker;
    private Node? node;
    private GLTFImport? gltf => node as GLTFImport;

    public override void _Ready()
    {
        node = Worker.WorkerDefinition?.Scene?.Instantiate();
        gltf?.AnimationPlayer?.Play("idle");
        node?.SetParent(this);
    }

    public override void _EnterTree()
    {

    }
}
