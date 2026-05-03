using Godot;
using Godot.Collections;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class RoomStateWorkerPath : Resource
{
    [Export]
    public uint TargetRoomId { get; set; }

    [Export]
    public Array<uint>? TransportsToTake { get; set; }
}