using Godot;
using Godot.Collections;
using System.Diagnostics;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/recipe-set-icon.svg")]
[GlobalClass]
[LoadDefinitions("res://recipes/sets")]
[DebuggerDisplay("{Name}")]
public partial class RecipeSetDefinition : Resource
{
    [Export]
    public Array<RecipeDefinition> Recipes { get; set; } = [];
}
