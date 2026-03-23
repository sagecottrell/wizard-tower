using Godot;
using Godot.Collections;
using System.Linq;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class TowerState : Resource, ICopy<TowerState>
{
    [Export]
    public Dictionary<int, FloorState> Floors { get; set; } = [];

    [Export]
    public Dictionary<uint, RoomState> Rooms { get; set; } = [];

    [Export]
    public uint RoomIdCounter { get; set; } = 0;

    [Export]
    public int LowestFloor { get; set; }

    [Export]
    public int HighestFloor { get; set; }

    public static bool operator ==(TowerState self, TowerState other) => self.Equals(other);

    public static bool operator !=(TowerState self, TowerState other) => !(self == other);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is null) return false;
        if (obj is not TowerState other) return false;
        return RoomIdCounter == other.RoomIdCounter && LowestFloor == other.LowestFloor && HighestFloor == other.HighestFloor && Floors.Keys.SequenceEqual(other.Floors.Keys) && Rooms.Keys.SequenceEqual(other.Rooms.Keys);
    }

    public override int GetHashCode() => base.GetHashCode();

    public TowerState Copy() => new()
    {
        Floors = Floors.Copy(),
        Rooms = Rooms.Copy(),
        RoomIdCounter = RoomIdCounter,
        LowestFloor = LowestFloor,
        HighestFloor = HighestFloor,
    };
}
