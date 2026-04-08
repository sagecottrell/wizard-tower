using Godot;

namespace wizardtower.containers;

[Tool]
public partial class RoomBackgroundScript : Node3D
{
    [Export]
    public MeshInstance3D? Background { get; set; }
    [Export]
    public Material? HologramMaterial { get; set; } 

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
