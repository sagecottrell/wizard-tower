using Godot;
using System;
using System.Collections.Generic;
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
        => FreeChildren(node, node.GetChildren(!owned), owned);
    public static void FreeChildren(this Node node, Func<Node, bool> predicate, bool owned = false) 
        => FreeChildren(node, node.GetChildren(!owned).Where(predicate), owned);
    public static void FreeChildren<TNode>(this Node node, Func<TNode, bool> predicate, bool owned = false) 
        => FreeChildren(node, node.GetChildren(!owned).Where(x => x is TNode n && predicate(n)), owned);
    public static void FreeChildren<TNode>(this Node node, bool owned = false) 
        where TNode : Node 
        => FreeChildren(node, node.GetChildren(!owned).Where(x => x is TNode), owned);

    /// <summary>
    /// Queues the specified child nodes of the given parent node for deletion and returns the children that were
    /// successfully queued.
    /// </summary>
    /// <remarks>Only child nodes whose parent is the specified <paramref name="node"/> are queued for
    /// deletion. Other nodes in the collection are ignored.</remarks>
    /// <param name="node">The parent node whose children are to be freed. Must not be null.</param>
    /// <param name="children">The collection of child nodes to attempt to free. Only children whose parent is <paramref name="node"/> will be
    /// affected.</param>
    /// <param name="owned">Indicates whether the parent node owns the children. This parameter is currently not used.</param>
    /// <returns>An enumerable collection of child nodes that were successfully queued for deletion.</returns>
    public static void FreeChildren(this Node node, IEnumerable<Node> children, bool owned = false)
    {
        foreach (var child in children)
            if (child.GetParent() == node)
                child.QueueFree();
    }

    public static SignalAwaiter GodotSleep(this Node node, float seconds) => node.ToSignal(node.GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);

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
