using Godot;


namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/worker-icon.svg")]
[GlobalClass]
public partial class WorkerDefinition : Resource, INamedResource<WorkerDefinition>
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
    public uint PlanningCapability { get; set; } = 4;

    [Export]
    public uint MovementSpeed { get; set; } = 4;

    [Export]
    public uint CarryCapacity { get; set; } = 2;


    [DefinitionLoader]
    public static System.Collections.Generic.Dictionary<string, WorkerDefinition> AllDefinitions => LoadDefs.LoadAll<WorkerDefinition>("res://workers/");
}
