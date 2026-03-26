using Godot;
using System.Diagnostics;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/tower-icon.svg")]
[DebuggerDisplay("{Name}")]
public partial class TowerDefinition : Resource, INamedResource<TowerDefinition>
{
    [Export]
    public string? Name { get; set; }

    public Texture2D? Icon { get; set; }
}
