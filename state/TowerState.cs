using Godot;
using Godot.Collections;
using System.Linq;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class TowerState : Resource, ICopy<TowerState>, IDeSerialize<TowerState>
{
    private uint maxWidth = 10;

    [Signal]
    public delegate void OnFloorAddEventHandler(FloorState floor);

    [Export]
    public Dictionary<int, FloorState> Floors { get; set; } = [];

    [ExportToolButton("Add Top Floor")]
    public Callable addTopFloor => Callable.From(AddTopFloor);

    [ExportToolButton("Add Basement Floor")]
    public Callable addBopFloor => Callable.From(AddBasementFloor);

    [Export]
    public Dictionary<uint, RoomState> Rooms { get; set; } = [];

    [Export]
    public Dictionary<uint, WorkerState> Workers { get; set; } = [];

    [Export]
    public Dictionary<uint, TransportState> Transports { get; set; } = [];

    [Export]
    public FloorDefinition? DefaultAboveGroundFloorDefinition { get; set; }

    [Export]
    public FloorDefinition? DefaultBasementFloorDefinition { get; set; }

    [Export]
    public uint RoomIdCounter { get; set; } = 0;

    [Export]
    public uint LowestFloor { get; set; } = 0;

    [Export]
    public uint HighestFloor { get; set; } = 0;

    [Export]
    public uint MaxHeight { get; set; }

    [Export]
    public uint MaxBasement { get; set; }

    [Export]
    public uint MaxWidth { 
        get => maxWidth; 
        set
        {
            maxWidth = value;
            foreach (var floor in Floors.Values)
                floor.MaxWidth = maxWidth;
        }
    }

    public void EnsureGroundFloor()
    {
        if (Floors.Count == 0)
        {
            Floors[0] = new()
            {
                TowerState = this,
                SizeLeft = 3,
                SizeRight = 3,
                Elevation = 0,
                Definition = DefaultAboveGroundFloorDefinition,
            };
        }
    }

    public void AddTopFloor()
    {
        EnsureGroundFloor();
        if (HighestFloor >= MaxHeight)
            return;
        HighestFloor++;
        Floors[(int)HighestFloor] = new()
        {
            TowerState = this,
            SizeLeft = 3,
            SizeRight = 3,
            Elevation = (int)HighestFloor,
            Definition = DefaultAboveGroundFloorDefinition,
        };
        EmitSignalOnFloorAdd(Floors[(int)HighestFloor]);
    }

    public void AddBasementFloor()
    {
        EnsureGroundFloor();
        if (LowestFloor >= MaxBasement)
            return;
        LowestFloor++;
        Floors[-(int)LowestFloor] = new()
        {
            TowerState = this,
            SizeLeft = 3,
            SizeRight = 3,
            Elevation = -(int)LowestFloor,
            Definition = DefaultBasementFloorDefinition ?? DefaultAboveGroundFloorDefinition,
        };
        EmitSignalOnFloorAdd(Floors[-(int)LowestFloor]);
    }

    public static bool operator ==(TowerState self, TowerState other) => self.Equals(other);

    public static bool operator !=(TowerState self, TowerState other) => !(self == other);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is null) return false;
        if (obj is not TowerState other) return false;
        return RoomIdCounter == other.RoomIdCounter 
            && LowestFloor == other.LowestFloor 
            && HighestFloor == other.HighestFloor 
            && Floors.Keys.SequenceEqual(other.Floors.Keys) 
            && Rooms.Keys.SequenceEqual(other.Rooms.Keys)
            && Transports.Keys.SequenceEqual(other.Transports.Keys)
            && Workers.Keys.SequenceEqual(other.Workers.Keys)
            && MaxBasement == other.MaxBasement
            && MaxHeight == other.MaxHeight
            && MaxWidth == other.MaxWidth;
    }

    public override int GetHashCode() => base.GetHashCode();

    public TowerState Copy() => new()
    {
        Floors = Floors.Duplicate(),
        Rooms = Rooms.Duplicate(),
        Transports = Transports.Duplicate(),
        Workers = Workers.Duplicate(),
        RoomIdCounter = RoomIdCounter,
        LowestFloor = LowestFloor,
        HighestFloor = HighestFloor,
        MaxBasement = MaxBasement,
        MaxHeight = MaxHeight,
        maxWidth = maxWidth,
    };

    public Dictionary<string, Variant> Serialize() => new()
    {
        { nameof(Floors), Floors.Serialize() },
        { nameof(Rooms), Rooms.Serialize() },
        { nameof(Transports), Transports.Serialize() },
        { nameof(Workers), Workers.Serialize() },
        { nameof(RoomIdCounter), RoomIdCounter },
        { nameof(LowestFloor), LowestFloor },
        { nameof(HighestFloor), HighestFloor },
        { nameof(MaxWidth), MaxWidth },
        { nameof(MaxHeight), MaxHeight },
        { nameof(MaxBasement), MaxBasement },
    };

    public TowerState Deserialize(Dictionary<string, Variant> dict)
    {
        Floors = dict[nameof(Floors)].AsSaveFormatDict().DeSerialize<int, FloorState>(int.Parse);
        Rooms = dict[nameof(Rooms)].AsSaveFormatDict().DeSerialize<uint, RoomState>(uint.Parse);
        Transports = dict[nameof(Transports)].AsSaveFormatDict().DeSerialize<uint, TransportState>(uint.Parse);
        Workers = dict[nameof(Workers)].AsSaveFormatDict().DeSerialize<uint, WorkerState>(uint.Parse);
        RoomIdCounter = dict[nameof(RoomIdCounter)].AsUInt32();
        LowestFloor = dict[nameof(LowestFloor)].AsUInt32();
        HighestFloor = dict[nameof(HighestFloor)].AsUInt32();
        MaxBasement = dict[nameof(MaxBasement)].AsUInt32();
        MaxWidth = dict[nameof(MaxWidth)].AsUInt32();
        MaxHeight = dict[nameof(MaxHeight)].AsUInt32();
        return this;
    }
}
