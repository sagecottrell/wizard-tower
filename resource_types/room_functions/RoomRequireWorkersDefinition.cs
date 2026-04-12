using Godot;

namespace wizardtower.resource_types.room_functions;

[Tool]
[GlobalClass]
public partial class RoomRequireWorkersDefinition : BaseRoomFunctionDefinition
{
    [Export]
    public NumericDict<WorkerDefinition, uint> Workers { get; set; } = [];
}
