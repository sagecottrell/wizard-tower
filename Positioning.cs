using Godot;
using System;

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

    public static void SkyBackground(this Node3D node) 
    { 
        node.Position = node.Position with { Z = -100 };
    }

    public static void GroundBackground(this Node3D node)
    {
        node.Position = node.Position with { Z = -10 };
    }

    public static void FloorBackground(this FloorScript node)
    {
        node.Position = node.Position with { Z = node.State.Elevation * -0.1f };
    }

    public static void RoomBackground(this Node3D node) 
    { 
        node.Position = node.Position with { Z = 1 };
    }
}


public enum RenderLayer
{
    SkyBackground = -100,
    GroundBackground = -10,
    FloorBackground = 0,
    RoomBackground = 1,
    RoomForeground = 2,
    Workers = 3,
    FloorForeground = 4,

    UI = 100,
}