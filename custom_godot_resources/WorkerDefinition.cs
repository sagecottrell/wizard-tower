using Godot;
using wizardtower.custom_godot_resources.containers;

namespace wizardtower.custom_godot_resources;

[Tool]
[Icon("res://custom_godot_resources/worker-icon.svg")]
[GlobalClass]
public partial class WorkerDefinition : Resource, INamedResource
{
    [Export]
    public string? Name { get; set; }

    [Export]
    public Texture2D? Icon { get; set; }


    [DefinitionLoader]
    public static System.Collections.Generic.Dictionary<string, WorkerDefinition> AllDefinitions => LoadDefs.LoadAll<WorkerDefinition>("res://workers/", r => r.Name);
}
