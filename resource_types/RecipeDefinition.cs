using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
    public uint ProcessingTimeSeconds { get; set; } = 10;

    [Export]
    public NumericDict<ItemDefinition, uint>? Input { get; set; }

    [Export]
    public Godot.Collections.Array<RandomOutputDefinition>? Output { get; set; }

    public NumericDict<ItemDefinition, float>? AverageItemOutputRate { get
        {
            if (Output is null)
                return null;
            var weightSum = Output.Sum(ro => ro.Weight);
            var pairs = Output.SelectMany(d => d.Output.Select(pair => (pair.Key, (float)pair.Value * d.Weight / weightSum / ProcessingTimeSeconds)));
            var sums = pairs.GroupBy(p => p.Key).Select(grouping => System.Collections.Generic.KeyValuePair.Create(grouping.Key, grouping.Sum(i => i.Item2)));
            return [.. sums];
        } }

    public IEnumerable<ItemDefinition> PossibleOutputs => Output?.SelectMany(r => r.Output.Keys).Distinct() ?? [];
}
