using Godot;
using System.Diagnostics;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/floor-icon.svg")]
[GlobalClass]
[LoadDefinitions("res://floors/")]
[DebuggerDisplay("{Name}")]
public partial class FloorDefinition : Resource, INamedResource<FloorDefinition>
{
    [Export]
    public string? Name { get; set; }

    [Export]
    public Texture2D? Icon { get; set; }

    [Export]
    public string? Readme { get; set; }

    [Export]
    public string? Description { get; set; }

    [Export]
    public PackedScene? FloorBackgroundTileScene { get; set; }

    [Export]
    public NumericDict<ItemDefinition, uint> CostToBuildPerUnit { get; set; } = [];
}
