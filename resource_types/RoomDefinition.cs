using Godot;
using Godot.Collections;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/room-icon.svg")]
[GlobalClass]
[LoadDefinitions("res://rooms/")]
public partial class RoomDefinition : Resource, INamedResource<RoomDefinition>
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
    public uint Width { get; set; } = 1;

    [Export]
    public uint Height { get; set; } = 1;

    [Export]
    public PackedScene? RoomScene { get; set; }

    [Export]
    public NumericDict<ItemDefinition, uint>? CostToBuildPerUnit { get; set; } = [];

    [Export]
    public Array<FloorDefinition> AllowedFloors { get; set; } = [];

    [Export]
    public NumericDict<RoomDefinition, NumericDict<ItemDefinition, uint>> Upgrades { get; set; } = [];
}
