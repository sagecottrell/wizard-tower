using Godot;
using Godot.Collections;
using wizardtower.custom_godot_resources.containers;

namespace wizardtower.custom_godot_resources;

[Tool]
[Icon("res://custom_godot_resources/tier-icon.svg")]
[GlobalClass]
public partial class TierDefinition : Resource, INamedResource
{
    [Export]
    public string? Name { get; set; }

    [Export]
    public Texture2D? Icon { get; set; }

    [Export]
    public string? Readme { get; set; }

    [Export]
    public Array<ResearchDefinition> ResearchUnlocked { get; set; } = [];

    [DefinitionLoader]
    public static System.Collections.Generic.Dictionary<string, TierDefinition> AllDefinitions => LoadDefs.LoadAll<TierDefinition>("res://tiers/", r => r.Name);
}
