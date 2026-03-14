using Godot;

namespace wizardtower.custom_godot_resources;

public interface INamedResource
{
    public string? Name { get; }

    public Texture2D? Icon { get; }
}
