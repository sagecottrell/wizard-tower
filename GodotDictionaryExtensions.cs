using Godot;
using Godot.Collections;
using System.Linq;

namespace wizardtower;

public static class GodotDictionaryExtensions
{
    public static Dictionary<TKey, TValue> Copy<[MustBeVariant] TKey, [MustBeVariant] TValue>(this Dictionary<TKey, TValue> dictionary)
        where TKey : notnull
        where TValue : ICopy<TValue>
        => new(dictionary.ToDictionary(k => k.Key, k => k.Value.Copy()));
}
