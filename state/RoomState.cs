using Godot;
using Godot.Collections;
using System.Diagnostics;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
[DebuggerDisplay("Room{Id}[e{Elevation}p{FloorPosition}n{Definition.Name}]")]
public partial class RoomState : Resource, ICopy<RoomState>, IDeSerialize<RoomState>
{
    [Export]
    public RoomDefinition Definition { get; set; } = new();

    [Export]
    public uint Id { get; set; }

    [Export]
    public int Elevation { get; set; }

    [Export]
    public uint Height { get; set; }

    [Export]
    public int FloorPosition { get; set; }

    [Export]
    public NumericDict<ItemDefinition, uint> StoredItems { get; set; } = [];

    [Export]
    public Array<RoomStateWorkerPath>? WorkerPaths { get; set; }

    public bool Compare(RoomState? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return Elevation == other.Elevation && Height == other.Height && FloorPosition == other.FloorPosition && Definition == other.Definition;
    }

    public RoomState Copy() => new()
    {
        Id = Id,
        Elevation = Elevation,
        Height = Height,
        FloorPosition = FloorPosition,
        Definition = Definition,
        StoredItems = StoredItems.Copy(),
    };

    public Dictionary<string, Variant> Serialize() => new()
    {
        { nameof(Definition), Definition?.ResourcePath ?? "" },
    };

    public RoomState Deserialize(Dictionary<string, Variant> dict)
    {
        Definition = LoadDefs.Get<RoomDefinition>(dict[nameof(Definition)].AsString()) ?? Definition;
        Elevation = dict[nameof(Elevation)].AsInt32();
        Height = dict[nameof(Height)].AsUInt32();
        FloorPosition = dict[nameof(FloorPosition)].AsInt32();
        return this;
    }

    public System.Collections.Generic.IEnumerable<RoomStateWorkerPath> GetWorkerPathTo(RoomState destination)
    {
        if (WorkerPaths is null)
            yield break;
        foreach (var path in WorkerPaths)
        {
            if (path.TargetRoomId == destination.Id)
                yield return path;
        }
    }
}
