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
    public Array<TransportToTake>? TransportsToTake { get; set; }

    [Export]
    public Array<float> TimeTakenRecords { get; set; } = [];
}

public partial class TransportToTake : Resource
{
    [Export]
    public uint TransportId { get; set; }

    [Export]
    public int Elevation { get; set; }

    /// <summary>
    /// If true, the Elevation field is required. otherwise, Elevation will be ignored
    /// </summary>
    [Export]
    public bool ElevationRequired { get; set; }
}