using Godot;
using System.Collections.Generic;

namespace wizardtower;

public partial class GLTFImport : Node3D
{
    public AnimationPlayer? AnimationPlayer { get; private set; }

    private readonly Dictionary<string, MeshInstance3D> meshes = [];
    public IReadOnlyDictionary<string, MeshInstance3D> Meshes => meshes;

    public override void _Ready()
    {
        foreach (var child in FindChildren("*", nameof(MeshInstance3D), true, false))
            if (child is MeshInstance3D mesh)
                meshes[child.GetParent().Name] = mesh;
        AnimationPlayer = this.Child<AnimationPlayer>();
    }

    public void ApplyMaterial(Material material)
    {
        foreach (var mesh in meshes.Values)
            mesh.SetSurfaceOverrideMaterial(0, material);
    }

    public void ResetMaterial()
    {
        foreach (var mesh in meshes.Values)
            mesh.SetSurfaceOverrideMaterial(0, null);
    }

    public void Play(StringName name)
    {
        if (AnimationPlayer is null || !AnimationPlayer.HasAnimation(name))
            return;
        AnimationPlayer.Play(name);
    }
}
