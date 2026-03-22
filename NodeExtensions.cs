using Godot;

namespace wizardtower;

public static class NodeExtensions
{
    public static Aabb GetBoundingBox(this Node3D node, bool excludeTopLevelTransform = true)
    {
        // Lights do not have bounding boxes that we care about
        var bounds = node is VisualInstance3D vi && vi is not Light3D ? vi.GetAabb() : new();

        foreach (var child in node.GetChildren())
        {
            // children that are not Node3D do not have bounding boxes
            if (child is not Node3D n3d)
                continue;
            bounds = bounds.Merge(GetBoundingBox(n3d, false));
        }

        if (!excludeTopLevelTransform)
            bounds = node.Transform * bounds;

        return bounds;
    }
}
