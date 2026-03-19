using Godot;

namespace wizardtower.custom_godot_resources;

public interface INamedResource
{
    string? Name { get; }

    Texture2D? Icon { get; }

    string? IconUid => ResourceUid.PathToUid(Icon?.ResourcePath);

    string? IconPathOrName => Icon?.ResourcePath ?? Name;
}

public interface INamedResource<TSelf> : INamedResource
{

}