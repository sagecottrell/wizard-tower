using Godot;

namespace wizardtower.resource_types.room_functions;

[Tool]
[GlobalClass]
public partial class RoomConvertResourcesDefinition : BaseRoomFunctionDefinition
{
    [Export]
    public RecipeDefinition Recipe { get; set; } = new();

    [Export]
    public bool ToTowerWallet { get; set; }

    [Export]
    public uint ProcessingTimeSeconds { get; set; } = 10;

    /// <summary>
    /// If zero, then there is no limit
    /// </summary>
    [Export]
    public uint MaxTimesPerDay { get; set; } = 0;

    [Export]
    public WorkerDefinition WorkerKind { get; set; } = new();

    [Export]
    public uint WorkersCount { get; set; } = 1;
}
