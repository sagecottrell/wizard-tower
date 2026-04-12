using Godot;
using System.Diagnostics;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/recipe-icon.svg")]
[GlobalClass]
[LoadDefinitions("res://recipes/")]
[DebuggerDisplay("{Name}")]
public partial class RecipeDefinition : Resource, INamedResource<RecipeDefinition>, IDebug
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
    public NumericDict<ItemDefinition, uint> Input { get; set; } = [];

    [Export]
    public Godot.Collections.Array<RandomOutputDefinition>? Output { get; set; }
}
