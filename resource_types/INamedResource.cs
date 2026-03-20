using Godot;
using System.Reflection;

namespace wizardtower.resource_types;

public interface INamedResource
{
    string? Name { get; }

    Texture2D? Icon { get; }

    string? IconUid => ResourceUid.PathToUid(Icon?.ResourcePath);

    string? IconPathOrName => Icon?.ResourcePath ?? GetType().GetCustomAttribute<IconAttribute>()?.Path ?? Name;
}

public interface INamedResource<TSelf> : INamedResource
{

}