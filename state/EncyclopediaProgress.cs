using Godot;
using Godot.Collections;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class EncyclopediaProgress : Resource
{
    [Export]
    public Array<RoomDefinition>? RoomsSeen { get; set; }

    [Export]
    public Array<TransportDefinition>? TransportsSeen { get; set; }

    [Export]
    public Array<FloorDefinition>? FloorsSeen { get; set; }

    [Export]
    public Array<ResearchDefinition>? ResearchDefinitionsSeen { get; set; }
}
