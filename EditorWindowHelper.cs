using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace wizardtower;

public static class EditorWindowHelper
{
    public static readonly Dictionary<int, Window> windows = [];

    public static int PopupEditor(Control control, Action? onQuit = null, Rect2I? rect = null)
    {
        rect ??= new(50, 50, 500, 500);
        var id = windows.Keys.DefaultIfEmpty().Max() + 1;

        Window window = new();
        window.CloseRequested += () => {
            window.QueueFree();
            onQuit?.Invoke();
        };
        EditorInterface.Singleton.PopupDialog(window, rect);
        window.AddChild(new EditorWindowControl() {
            Id = id,
            Window = window,
            AnchorBottom = 1,
            AnchorRight = 1,
            GrowHorizontal = Control.GrowDirection.Both,
            GrowVertical = Control.GrowDirection.Both,
        }.WithChild(control));

        windows[id] = window;
        return id;
    }

    public static void Close(int id)
    {
        if (windows.TryGetValue(id, out var window))
        {
            window.QueueFree();
            windows.Remove(id);
        }
    }


}

public partial class EditorWindowControl : Control
{
    public int Id { get; set; }
    public Window? Window { get; set; }

    public override void _Process(double delta)
    {
        if (!EditorWindowHelper.windows.TryGetValue(Id, out var existingWindow) || existingWindow != Window)
        {
            GD.Print($"window is no longer registered in helper, overwriting registration");
            // most likely, the assembly was reloaded while this window is open
            existingWindow?.QueueFree();
            EditorWindowHelper.windows[Id] = Window!;
        }
    }
}