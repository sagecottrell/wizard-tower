using Godot;


namespace wizardtower.custom_godot_resources;

[Tool]
[Icon("res://custom_godot_resources/transport-icon.svg")]
[GlobalClass]
public partial class TransportDefinition : Resource, INamedResource
{
    [Export]
    public string? Name { get; set; }

    [Export]
    public Texture2D? Icon { get; set; }

    [DefinitionLoader]
    public static System.Collections.Generic.Dictionary<string, TransportDefinition> AllDefinitions => LoadDefs.LoadAll<TransportDefinition>("res://transports/", r => r.Name);

}
