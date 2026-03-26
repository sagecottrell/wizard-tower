using Godot;
using Godot.Collections;
using System.Diagnostics;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/tier-icon.svg")]
[GlobalClass]
[LoadDefinitions("res://tiers/")]
[DebuggerDisplay("{Name}")]
public partial class TierDefinition : Resource, INamedResource<TierDefinition>
{
    [Export]
    public string? Name { get; set; }

    [Export]
    public Texture2D? Icon { get; set; }

    [Export]
    public string? Readme { get; set; }

    [Export]
    public Array<ResearchDefinition> ResearchUnlocked { get; set; } = [];

    /// <summary>
    /// gift to the tower wallet upon reaching this tier
    /// </summary>
    [Export]
    public NumericDict<ItemDefinition, uint> GiftForTower { get; set; } = [];

    /// <summary>
    /// gift to the player account wallet upon reaching this tier
    /// </summary>
    [Export]
    public NumericDict<ItemDefinition, uint> GiftForSaveFile { get;set; } = [];
}
