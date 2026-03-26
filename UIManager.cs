using Godot;
using System.Collections.Generic;

namespace wizardtower;

[GlobalClass]
public partial class UIManager : CanvasLayer
{
    [Export]
    public Control? Node;

    [Export]
    /// if true, other UI components should restore this one when they hide themselves
    public bool OthersCanStack = false;

    static UIManager? currentDisplay;
    static readonly Stack<UIManager> displayStack = [];
    Control? ui;

    public override void _Ready()
    {
        Node ??= this.ChildControl();
        this.Log($"Node={Node}");
        if (Node is not null && Node.GetParent() == this)
            RemoveChild(Node);
    }

    public void ShowUI()
    {
        var scene = Node;
        if (scene is null)
            return;
        if (currentDisplay is UIManager c)
        {
            if (c.OthersCanStack) c.HideStackableUI();
            else c.RemoveUI();
        }
        currentDisplay = this;
        ui ??= scene;
        AddChild(ui);
    }

    public void RemoveUI()
    {
        currentDisplay = null;
        RemoveChild(ui);
        ui = null;

        if (displayStack.TryPop(out var nextDisplay))
        {
            nextDisplay.ShowUI();
        }
    }

    public void HideStackableUI()
    {
        displayStack.Push(this);
        RemoveChild(ui);
    }

    public static bool IsDisplaying() => currentDisplay != null;
}
