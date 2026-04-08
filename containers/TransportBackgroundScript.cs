using Godot;

namespace wizardtower.containers;

[Tool]
public partial class TransportBackgroundScript : Node3D
{
    [Export]
    public MeshInstance3D? Background { get; set; }
    [Export]
    public Material? HologramMaterial { get; set; }

    public TransportBackgroundScript AsHologram()
    {
        Background?.SetSurfaceOverrideMaterial(0, HologramMaterial);
        return this;
    }

    public TransportBackgroundScript AsBackground()
    {
        Background?.SetSurfaceOverrideMaterial(0, null);
        return this;
    }
}
