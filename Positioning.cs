using Godot;

namespace wizardtower;

public static class Positioning
{
    public static Vector3 TowerCoordToNodePosition(this Node3D node, int? x = null, int? y = null)
    {
        var np = node.Position;
        if (x is int X)
            np.X = X - 0.5f * Mathf.Sign(X);
        if (y is int Y)
            np.Y = Y;
        return np;
    }
}
