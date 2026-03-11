using Godot;
using System;
using System.Collections.Generic;

namespace wizardtower.custom_godot_resources;

public static class LoadDefs
{
    public static Dictionary<string, T> LoadAll<[MustBeVariant] T>(ref Dictionary<string, T>? _allDefinitions, string ResourcePathFormat, Func<T, string> map, string? extension = "res") where T : class
    {
        if (_allDefinitions != null && !Engine.IsEditorHint())
            return _allDefinitions;
        _allDefinitions = [];
        var resourceLoader = ResourceLoader.Singleton;

        foreach (var fileName in DirAccess.GetFilesAt(ResourcePathFormat))
        {
            if (!string.IsNullOrWhiteSpace(extension) && !fileName.EndsWith(extension))                 
                continue;
            var resourcePath = ResourcePathFormat + fileName;
            if (resourceLoader.Load(resourcePath) is T resource)
            {
                var id = map(resource);
                if (string.IsNullOrWhiteSpace(id))
                    continue;
                _allDefinitions[id] = resource;
            }
        }
        return _allDefinitions;
    }
}
