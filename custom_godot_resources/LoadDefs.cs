using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace wizardtower.custom_godot_resources;

public static class LoadDefs
{
    private static readonly Dictionary<Type, IDictionary> _allDefinitionsForEachType = [];

    public static Dictionary<string, T> LoadAll<[MustBeVariant] T>(string ResourcePathFormat, Func<T, string?> map, string? extension = "res") where T : class
    {
        if (!_allDefinitionsForEachType.TryGetValue(typeof(T), out var allDefinitions))
        {
            var d = new Dictionary<string, T>();
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
                    d[id] = resource;
                }
            }

            if (!Engine.IsEditorHint())
                _allDefinitionsForEachType[typeof(T)] = d;

            return d;
        }
        return (Dictionary<string, T>)allDefinitions;
    }
}
