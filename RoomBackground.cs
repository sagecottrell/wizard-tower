using Godot;
using wizardtower.state;

namespace wizardtower;

[Tool]
public partial class RoomBackground : Node3D
{
    [Export]
    public MeshInstance3D? Background { get; set; }
    [Export]
    public Material? HologramMaterial { get; set; } 

    public RoomState RoomState { get; set; } = new();

    public RoomBackground AsHologram()
    {
        Background?.SetSurfaceOverrideMaterial(0, HologramMaterial);
        return this;
    }

    public RoomBackground AsBackground()
    {
        Background?.SetSurfaceOverrideMaterial(0, null);
        return this;
    }
}
