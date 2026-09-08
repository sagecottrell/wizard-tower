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
    public Array<TransportToTake> TransportsToTake { get; set; } = [];

    [Export]
    public Array<float> TimeTakenRecords { get; set; } = [];

    public virtual bool TryGetFinalPosition(TowerState tower, out Vector3 position)
    {
        if (tower.Rooms.TryGetValue(TargetRoomId, out var toRoom))
        {
            position = new(toRoom.FloorPosition, toRoom.Elevation, 0);
            return true;
        }
        position = Vector3.Zero;
        return false;
    }
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

public partial class PartialRoomStateWorkerPath : RoomStateWorkerPath
{
    public Vector3 FinalPosition { get; set; }


    public override bool TryGetFinalPosition(TowerState tower, out Vector3 position)
    {
        position = FinalPosition;
        return true;
    }
}