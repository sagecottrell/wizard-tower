using Godot;
using wizardtower.events.handlers;
using wizardtower.events.Room;

namespace wizardtower.containers;

[Tool]
public partial class RoomBackgroundScript : Node3D
{
    [Export]
    public MeshInstance3D? Background { get; set; }
    [Export]
    public Material? HologramMaterial { get; set; }

    [Export]
    public AnimationPlayer? Animations { get; set; }

    public override void _EnterTree()
    {
        RoomEvents.StartedWork += _startedWork;

        Animations?.Play("idle");
    }

    public override void _ExitTree()
    {
        RoomEvents.StartedWork -= _startedWork;
    }

    void _startedWork(RoomStartedWorkEvent ev)
    {
    }

    public RoomBackgroundScript AsHologram()
    {
        Background?.SetSurfaceOverrideMaterial(0, HologramMaterial);
        return this;
    }

    public RoomBackgroundScript AsBackground()
    {
        Background?.SetSurfaceOverrideMaterial(0, null);
        return this;
    }
}
