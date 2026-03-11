using Godot;

namespace wizardtower.custom_godot_resources;

public interface INamedResource
{
    public string ItemName { get; }

    public Texture2D? Icon { get; }
}
