using Godot;
using Godot.Collections;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/research-icon.svg")]
[GlobalClass]
public partial class ResearchDefinition : Resource, INamedResource<ResearchDefinition>
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
    public NumericDict<ItemDefinition, uint>? CostToResearch { get; set; }

    [Export]
    public Array<ResearchDefinition> Prerequisites { get; set; } = [];

    [Export]
    public Array<FloorDefinition> UnlocksFloors { get; set; } = [];

    [Export]
    public Array<TransportDefinition> UnlocksTransports { get; set; } = [];

    [Export]
    public Array<RoomDefinition> UnlocksRooms { get; set; } = [];

    [Export]
    public uint MaxTimesResearchable { get; set; } = 1;


    [DefinitionLoader]
    public static System.Collections.Generic.Dictionary<string, ResearchDefinition> AllDefinitions => LoadDefs.LoadAll<ResearchDefinition>("res://research/");
}
