using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace wizardtower.resource_types;

public static class LoadDefs
{
    private static readonly Dictionary<Type, IDictionary> _allDefinitionsForEachType = [];

    public static T? Get<[MustBeVariant] T>(string path) where T : Resource
    {
        if (LoadAll<T>().TryGetValue(path, out var value))
            return value;
        GD.PrintErr($"Failed to load {typeof(T).FullName}: \"{path}\"");
        return null;
    }

    public static IReadOnlyDictionary<string, T> LoadAll<[MustBeVariant] T>() where T : Resource
    {
        if (!_allDefinitionsForEachType.TryGetValue(typeof(T), out var allDefinitions))
        {
            if (typeof(T).GetCustomAttribute<LoadDefinitionsAttribute>() is not LoadDefinitionsAttribute lda)
            {
                var dict = new Dictionary<string, T>();
                _allDefinitionsForEachType[typeof(T)] = dict;
                return dict;
            }

            var d = new Dictionary<string, T>();
            var resourceLoader = ResourceLoader.Singleton;

            foreach (var fileName in DirAccess.GetFilesAt(lda.Path))
            {
                if (!string.IsNullOrWhiteSpace(lda.Extension) && !fileName.EndsWith(lda.Extension))
                    continue;
                var resourcePath = lda.Path + fileName;
                if (resourceLoader.Load(resourcePath) is T resource && resource.ResourcePath is not null)
                {
                    d[resource.ResourcePath] = resource;
                }
            }

            if (!Engine.IsEditorHint())
                _allDefinitionsForEachType[typeof(T)] = d;

            return d;
        }
        return (Dictionary<string, T>)allDefinitions;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class LoadDefinitionsAttribute(string ResourcePathFormat, string extension = "tres") : Attribute
{
    public string Path { get; } = ResourcePathFormat;
    public string Extension { get; } = extension;
}