using Godot;
using Godot.Collections;
using wizardtower.custom_godot_resources.containers;
using wizardtower.enums;

namespace wizardtower.custom_godot_resources;

[Tool]
[Icon("res://custom_godot_resources/room-icon.svg")]
[GlobalClass]
public partial class RoomDefinition : Resource, INamedResource
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
    public RoomCategory Category { get; set; } = RoomCategory.None;

    [Export]
    public uint Width { get; set; } = 1;

    [Export]
    public uint Height { get; set; } = 1;

    [Export]
    public PackedScene? RoomScene { get; set; }

    [Export]
    public ItemContainer? CostToBuildPerUnit { get; set; }

    [Export]
    public Array<FloorDefinition> AllowedFloors { get; set; } = [];

    [Export]
    public Array<RoomDefinition> Upgrades { get; set; } = [];

    [DefinitionLoader]
    public static System.Collections.Generic.Dictionary<string, RoomDefinition> AllDefinitions => LoadDefs.LoadAll<RoomDefinition>("res://rooms/", r => r.Name);
}
