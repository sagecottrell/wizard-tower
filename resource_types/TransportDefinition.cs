using Godot;
using Godot.Collections;
using System.Diagnostics;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/transport-icon.svg")]
[GlobalClass]
[LoadDefinitions("res://transports/")]
[DebuggerDisplay("{Name}")]
public partial class TransportDefinition : Resource, INamedResource<TransportDefinition>
{
    [Export]
    public string? Name { get; set; }

    [Export]
    public Texture2D? Icon { get; set; }

    [Export]
    public string? Description { get; set; }

    [Export]
    public string? Readme { get; set; }

    [Export]
    public PackedScene? Scene { get; set; }

    [Export]
    public uint MinHeight { get; set; } = 2;

    [Export]
    public uint MaxHeight { get; set; } = 10;

    [Export]
    public uint Width { get; set; } = 1;

    [Export]
    public Array<FloorDefinition> CanStopAtFloor { get; set; } = [];

    [Export]
    public NumericDict<ItemDefinition, uint> CostToBuild { get; set; } = [];

}
