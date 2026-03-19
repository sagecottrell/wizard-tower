using Godot;
using Godot.Collections;


namespace wizardtower.custom_godot_resources;

[Tool]
[Icon("res://custom_godot_resources/transport-icon.svg")]
[GlobalClass]
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
    public Array<FloorDefinition> CanStopAtFloor { get; set; } = [];

    [DefinitionLoader]
    public static System.Collections.Generic.Dictionary<string, TransportDefinition> AllDefinitions => LoadDefs.LoadAll<TransportDefinition>("res://transports/");

}
