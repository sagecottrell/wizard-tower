using Godot;
using Godot.Collections;

namespace wizardtower.resource_types.room_functions;

[Tool]
[GlobalClass]
public partial class RoomConvertResources : BaseRoomFunction
{
    [Export]
    public NumericDict<ItemDefinition, uint> Input { get; set; } = [];

    [Export]
    public Array<RandomOutput> Output { get; set; }
}
