using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace wizardtower.resource_types;

public static class LoadDefs
{
    private static readonly Dictionary<Type, IDictionary> _allDefinitionsForEachType = [];

    public static Dictionary<string, T> LoadAll<[MustBeVariant] T>(string ResourcePathFormat, string? extension = "res") where T : class, INamedResource
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
                if (resourceLoader.Load(resourcePath) is T resource && resource.Name is not null)
                {
                    d[resource.Name] = resource;
                }
            }

            if (!Engine.IsEditorHint())
                _allDefinitionsForEachType[typeof(T)] = d;

            return d;
        }
        return (Dictionary<string, T>)allDefinitions;
    }
}
