using Godot;

namespace wizardtower.resource_types.room_functions;

[Tool]
[GlobalClass]
public partial class RoomRequireWorkers : BaseRoomFunction
{
    [Export]
    public NumericDict<WorkerDefinition, uint> Workers { get; set; } = [];
}
