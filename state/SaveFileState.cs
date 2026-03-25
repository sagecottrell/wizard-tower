using Godot;
using Godot.Collections;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class SaveFileState : Resource, IDeSerialize<SaveFileState>
{
    [Export]
    public NumericDict<ItemDefinition, uint> Wallet { get; set; } = [];

    /// <summary>
    /// A list of the currently "alive" towers, i.e. have not yet reached the end of the final day. otherwise, towers with the win condition.
    /// </summary>
    [Export]
    public Array<TowerState>? AliveTowers { get; set; }

    /// <summary>
    /// once a tower reaches the end of the final day, it is moved to this array of dead towers unless the win condition was met.
    /// </summary>
    [Export]
    public Array<TowerState>? DeadTowers { get; set; }

    [Export]
    public EncyclopediaProgress? EncyclopediaProgress { get; set; }

    [Export]
    public NumericDict<ResearchDefinition, uint> Research { get; set; } = [];

    public SaveFileState Deserialize(Dictionary<string, Variant> dict) => this;

    public Dictionary<string, Variant> Serialize() => [];
}
