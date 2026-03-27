using Godot;
using Godot.Collections;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class EncyclopediaProgress : Resource, ICopy<EncyclopediaProgress>, IDeSerialize<EncyclopediaProgress>
{
    [Export]
    public NumericDict<RoomDefinition, int> RoomsSeen { get; set; } = [];

    [Export]
    public NumericDict<TransportDefinition, int> TransportsSeen { get; set; } = [];

    [Export]
    public NumericDict<FloorDefinition, int> FloorsSeen { get; set; } = [];

    [Export]
    public NumericDict<ResearchDefinition, int> ResearchDefinitionsSeen { get; set; } = [];

    public EncyclopediaProgress Copy() => new()
    {
        RoomsSeen = RoomsSeen.Copy(),
        TransportsSeen = TransportsSeen.Copy(),
        FloorsSeen = FloorsSeen.Copy(),
        ResearchDefinitionsSeen = ResearchDefinitionsSeen.Copy(),
    };

    public Dictionary<string, Variant> Serialize() => [];

    public EncyclopediaProgress Deserialize(Dictionary<string, Variant> dict) => this;

}
