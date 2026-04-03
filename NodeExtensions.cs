using Godot;
using System;
using System.Linq;

namespace wizardtower;

public static class NodeExtensions
{
    public static TNode WithChild<TNode>(this TNode node, Node child, bool owned = false)
        where TNode : Node
    {
        node.AddChild(child);
        if (owned)
            child.Owner = node.Owner;
        return node;
    }

    public static TChild AddedChild<TChild>(this Node node, TChild child, bool owned = false)
        where TChild : Node
    {
        node.AddChild(child);
        if (owned)
            child.Owner = node.Owner;
        return child;
    }

    public static TNode Configured<TNode>(this TNode node, Action<TNode> configure)
        where TNode : Node
    {
        configure(node);
        return node;
    }

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

    public static TNode? Child<TNode>(this Node node, string name = "", bool owned = false) 
        where TNode : Node
        => node.GetChildren(!owned).FirstOrDefault(x => x.Name.ToString().Contains(name) && x is TNode) as TNode;
    public static Node3D? Child3D(this Node node, string name = "", bool owned = false) => node.Child<Node3D>(name, owned);
    public static Node2D? Child2D(this Node node, string name = "", bool owned = false) => node.Child<Node2D>(name, owned);
    public static Control? ChildControl(this Node node, string name = "", bool owned = false) => node.Child<Control>(name, owned);
    public static Node? Child(this Node node, string name = "", bool owned = false) => node.Child<Node>(name, owned);

    public static void FreeChildren(this Node node, bool owned = false)
    {
        foreach (var child in node.GetChildren(!owned))
        {
            if (child is Node n)
                n.QueueFree();
        }
    }

    public static void FreeChildren(this Node node, Func<Node, bool> predicate, bool owned = false)
    {
        foreach (var child in node.GetChildren(!owned))
        {
            if (child is Node n && predicate(n))
                n.QueueFree();
        }
    }

    public static void FreeChildren<TNode>(this Node node, Func<TNode, bool> predicate, bool owned = false) 
        where TNode : Node
    {
        foreach (var child in node.GetChildren(!owned))
        {
            if (child is TNode n && predicate(n))
                n.QueueFree();
        }
    }

    public static void FreeChildren<TNode>(this Node node, bool owned = false) 
        where TNode : Node
    {
        foreach (var child in node.GetChildren(!owned))
        {
            if (child is TNode n)
                n.QueueFree();
        }
    }

    public static SignalAwaiter GodotSleep(this Node node, float seconds) => node.ToSignal(node.GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);

    public static T AddOwnedChild<T>(this Node node, T child) where T : Node
    {
        node.AddChild(child);
        child.Owner = node.Owner ?? node;
        return child;
    }

    public static void Log(this Node node, string message)
    {
        var path = node.IsInsideTree() ? node.GetPath().ToString() : $"{node.GetType()} not in tree";
        GD.Print($"{DateTime.Now}|{path}|{message}");
    }

    public static void Error(this Node node, string message)
    {
        var path = node.IsInsideTree() ? node.GetPath().ToString() : $"{node.GetType()} not in tree";
        GD.PushError($"{DateTime.Now}|{path}|{message}");
    }


}
