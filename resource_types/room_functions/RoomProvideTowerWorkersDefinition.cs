using Godot;

namespace wizardtower.resource_types.room_functions;

[Tool]
[GlobalClass]
public partial class RoomProvideTowerWorkersDefinition : BaseRoomFunctionDefinition
{
    [Export]
    public NumericDict<WorkerDefinition, uint> WorkersToProduce { get; set; } = [];
}
