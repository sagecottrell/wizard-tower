using Godot;

namespace wizardtower.resource_types.room_functions;

[Tool]
[GlobalClass]
public partial class RoomConvertResourcesDefinition : Resource
{
    [Export]
    public RecipeSetDefinition Recipes { get; set; } = new();

    [Export]
    public bool ToTowerWallet { get; set; }

    [Export]
    public float ProcessingTimeMultiplier { get; set; } = 1;

    /// <summary>
    /// If zero, then there is no limit
    /// </summary>
    [Export]
    public uint MaxTimesPerDay { get; set; } = 0;

    [Export]
    public WorkerDefinition? WorkerKind { get; set; }

    [Export]
    public uint WorkersCount { get; set; } = 1;
}
