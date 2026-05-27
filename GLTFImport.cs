using Godot;
using System.Collections.Generic;

namespace wizardtower;

public partial class GLTFImport : Node
{
    public AnimationPlayer? AnimationPlayer { get; private set; }

    Dictionary<string, MeshInstance3D> meshes = [];
    public IReadOnlyDictionary<string, MeshInstance3D> Meshes => meshes;

    public override void _Ready()
    {
        foreach (var child in FindChildren("mesh"))
            if (child is MeshInstance3D mesh)
                meshes[child.GetParent().Name] = mesh;
        AnimationPlayer = this.Child<AnimationPlayer>();
    }
}
