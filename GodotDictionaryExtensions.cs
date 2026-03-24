using Godot;
using Godot.Collections;
using System;
using System.Linq;

namespace wizardtower;

public static class GodotDictionaryExtensions
{
    public static Dictionary<TKey, TValue> CopyRecursive<[MustBeVariant] TKey, [MustBeVariant] TValue>(this Dictionary<TKey, TValue> dictionary)
        where TKey : notnull
        where TValue : ICopy<TValue>
        => new(dictionary.ToDictionary(k => k.Key, static k => k.Value.Copy()));

    public static Dictionary<string, Variant> Serialize<[MustBeVariant] TKey, [MustBeVariant] TValue>(this Dictionary<TKey, TValue> dictionary)
        where TKey : notnull
        where TValue : IDeSerialize<TValue>
        => new(dictionary.ToDictionary(k => $"{k.Key}", static k => Variant.From(k.Value.Serialize())));

    public static Dictionary<string, Variant> AsSaveFormatDict(this Variant variant) => variant.As<Dictionary<string, Variant>>();

    public static Dictionary<TKey, TValue> DeSerialize<[MustBeVariant] TKey, [MustBeVariant] TValue>(this Dictionary<string, Variant> dictionary, Func<string, TKey> keymap)
        where TKey : notnull
        where TValue : IDeSerialize<TValue>, new()
        => new(dictionary.ToDictionary(kvp => keymap(kvp.Key), kvp => new TValue().Deserialize(kvp.Value.AsSaveFormatDict())));
}
