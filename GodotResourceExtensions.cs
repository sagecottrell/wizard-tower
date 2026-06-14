using Godot;

namespace wizardtower;

public static class GodotResourceExtensions
{
    public static T RDuplicate<T>(this T t, bool deep = false) where T : Resource => (T)t.Duplicate(deep);
}
