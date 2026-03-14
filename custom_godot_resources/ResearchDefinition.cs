using Godot;
using Godot.Collections;
using wizardtower.custom_godot_resources.containers;

namespace wizardtower.custom_godot_resources;

[Tool]
[Icon("res://custom_godot_resources/research-icon.svg")]
[GlobalClass]
public partial class ResearchDefinition : Resource, INamedResource
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
    public ItemContainer? CostToResearch { get; set; }

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
    public static System.Collections.Generic.Dictionary<string, ResearchDefinition> AllDefinitions => LoadDefs.LoadAll<ResearchDefinition>("res://research/", r => r.Name);
}
