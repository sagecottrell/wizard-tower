using Godot;
using System;
using System.Diagnostics.CodeAnalysis;

namespace wizardtower;

public static class SceneLoader
{
    private static readonly System.Collections.Generic.Dictionary<Type, PackedScene?> cache = [];

    public static bool TryLoadScene<T>([NotNullWhen(true)] out T? node, bool cacheError = true)
        where T : Node
    {
        node = default;
        var type = typeof(T);

        if (cache.TryGetValue(type, out var existing))
        {
            if (existing is null)
            {
                GD.PrintErr($"Type {type.FullName} previous load attempt failed");
                return false;
            }
            node = existing.Instantiate<T>();
            return true;
        }

        if (type.Namespace is not string ns)
        {
            GD.PrintErr($"Type {type.Name} does not have a namespace, cannot load a scene");
            if (cacheError)
                cache[type] = null;
            return false;
        }

        if (!ns.StartsWith("wizardtower."))
        {
            GD.PrintErr($"Type {type.FullName} is not in wizardtower namespace, cannot load a scene");
            if (cacheError)
                cache[type] = null;
            return false;
        }

        var parts = ns.Split(".")[1..];
        var x = string.Join("/", parts);

        var scene = ResourceLoader.Load<PackedScene>($"res://{x}/{type.Name}.tscn");
        if (scene is null)
        {
            GD.PrintErr($"Type {type.FullName} could not find co-located scene file with same name");
            if (cacheError)
                cache[type] = null;
            return false;
        }

        if (scene.Instantiate<T>() is not T instance)
        {
            GD.PrintErr($"Type {type.FullName} scene was not instantiated with correct type");
            if (cacheError)
                cache[type] = null;
            return false;
        }

        node = instance;
        return true;
    }
}
