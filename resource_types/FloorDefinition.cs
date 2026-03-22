using Godot;
using System.Collections.Generic;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/floor-icon.svg")]
[GlobalClass]
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
    public NumericDict<ItemDefinition, uint>? CostToBuildPerUnit { get; set; } = [];

    [DefinitionLoader]
    public static Dictionary<string, FloorDefinition> AllDefinitions => LoadDefs.LoadAll<FloorDefinition>("res://floors/");
}
