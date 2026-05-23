using Godot;
using Godot.Collections;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class RoomStateWorkerPath : Resource
{
    [Export]
    public uint TargetRoomId { get; set; }

    [Export]
    public ItemDefinition ItemDefinition { get; set; } = new();

    [Export]
    public Array<uint>? TransportsToTake { get; set; }

    [Export]
    public Array<int>? ToWhichFloors { get; set; }

    [Export]
    public Array<float> TimeTakenRecords { get; set; } = [];
}