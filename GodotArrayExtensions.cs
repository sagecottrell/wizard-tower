using Godot;
using Godot.Collections;
using System.Linq;

namespace wizardtower;

public static class GodotArrayExtensions
{
    public static Array<T> CopyRecursive<[MustBeVariant] T>(this Array<T> array)
        where T : ICopy<T>
        => [..array.Select(x => x.Copy())];
}
