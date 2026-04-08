using Godot;
using System;

namespace wizardtower;

public static class GodotObjectExtensions
{

    public static void Log(this GodotObject node, string message)
    {
        GD.Print($"{DateTime.Now}|{message}");
    }

    public static void Error(this GodotObject node, string message)
    {
        GD.PushError($"{DateTime.Now}|{message}");
    }

    public static void Debug(this GodotObject node, string message)
    {
        if (Input.IsActionPressed(InputMapConstants.LogGlobalSignals))
            GD.Print($"{DateTime.Now}|{message}");
    }
}
