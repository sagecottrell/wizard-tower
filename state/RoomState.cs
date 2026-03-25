using Godot;
using Godot.Collections;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class RoomState : Resource, ICopy<RoomState>, IDeSerialize<RoomState>
{
    [Export]
    public RoomDefinition? Definition { get; set; }

    [Export]
    public uint Id { get; set; }

    [Export]
    public int Elevation { get; set; }

    [Export]
    public uint Height { get; set; }

    [Export]
    public int FloorPosition { get; set; }

    public static bool operator ==(RoomState self, RoomState other) => self.Equals(other);

    public static bool operator !=(RoomState self, RoomState other) => !(self == other);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is null) return false;
        if (obj is not RoomState other) return false;
        return Elevation == other.Elevation && Height == other.Height && FloorPosition == other.FloorPosition && Definition == other.Definition;
    }

    public override int GetHashCode() => base.GetHashCode();

    public RoomState Copy()
    {
        return new()
        {
            Id = Id,
            Elevation = Elevation,
            Height = Height,
            FloorPosition = FloorPosition,
            Definition = Definition,
        };
    }

    public Dictionary<string, Variant> Serialize() => new()
    {
        { nameof(Definition), Definition?.ResourcePath ?? "" },
    };

    public RoomState Deserialize(Dictionary<string, Variant> dict)
    {
        Definition = LoadDefs.Get<RoomDefinition>(dict[nameof(Definition)].AsString());
        return this;
    }
}
