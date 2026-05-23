using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace wizardtower;

public static class ICollectionExtensions
{
    public static T? PickWeightedRandom<T>(this ICollection<T> values, RandomNumberGenerator random, Func<T, int> map)
    {
        if (values.Count <= 1)
            return values.FirstOrDefault();
        var sum = values.Sum(map);
        var rand = random.RandiRange(0, sum);
        foreach (var value in values)
        {
            rand -= map(value);
            if (rand <= 0)
                return value;
        }
        return values.FirstOrDefault();
    }

    public static T? PickRandom<T>(this ICollection<T> values, RandomNumberGenerator random)
    {
        if (values.Count <= 1)
            return values.FirstOrDefault();
        return values.ElementAt(random.RandiRange(0, values.Count - 1));
    }
}
