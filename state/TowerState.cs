using Godot;
using Godot.Collections;
using System.Linq;
using wizardtower.resource_types;
using wizardtower.UIs.editor_add_room;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class TowerState : Resource, ICopy<TowerState>, IDeSerialize<TowerState>, IDebug
{
    private uint maxWidth = 10;

    private System.Collections.Generic.HashSet<(int elevation, int position)>? vacancies;

    [Export]
    public string Name { get; set; } = "Tower";

    [Export]
    public Dictionary<int, FloorState> Floors { get; set; } = [];

    [ExportToolButton("Add Top Floor")]
    public Callable addTopFloor => Callable.From(AddTopFloor);

    [ExportToolButton("Add Basement Floor")]
    public Callable addBopFloor => Callable.From(AddBasementFloor);

    [Export]
    public Dictionary<uint, RoomState> Rooms { get; set; } = [];

    [ExportToolButton("Add New Room")]
    public Callable addroom => Callable.From(EditorAddRoom);

    [Export]
    public Dictionary<uint, WorkerState> Workers { get; set; } = [];

    [Export]
    public Dictionary<uint, TransportState> Transports { get; set; } = [];

    [Export]
    public FloorDefinition DefaultAboveGroundFloorDefinition { get; set; } = new();

    [Export]
    public FloorDefinition DefaultBasementFloorDefinition { get; set; } = new();

    [Export]
    public NumericDict<ItemDefinition, uint> Wallet { get; set; } = [];

    [Export]
    public Array<RoomDefinition> UnlockedRooms { get; set; } = [];

    [Export]
    public Array<FloorDefinition> UnlockedFloors { get; set; } = [];

    [Export]
    public Array<TransportDefinition> UnlockedTransports { get; set; } = [];

    [Export]
    public uint RoomIdCounter { get; set; } = 0;

    [Export]
    public uint LowestFloor { get; set; } = 0;

    [Export]
    public uint HighestFloor { get; set; } = 0;

    [Export]
    public uint MaxHeight { get; set; } = 5;

    [Export]
    public uint MaxBasement { get; set; } = 0;

    [Export]
    public uint MaxWidth
    {
        get => maxWidth;
        set
        {
            maxWidth = value;
            foreach (var floor in Floors.Values)
                floor.MaxWidth = maxWidth;
        }
    }


    #region Room functions

    public void EditorAddRoom()
    {
        if (SceneLoader.TryLoadScene<EditorAddRoom>(out var node))
        {
            node.TowerState = this;
            var id = EditorWindowHelper.PopupEditor(node);
            node.OnSave += (e, p, r) =>
            {
                EditorWindowHelper.Close(id);
                OnAddRoom(e, p, r);
            };
        }
    }

    public void OnAddRoom(int elevation, int position, RoomDefinition r)
    {
        GD.Print($"Add room {elevation} {position} {r}");
        var room = new RoomState()
        {
            Id = RoomIdCounter + 1,
            Definition = r,
            Elevation = elevation,
            FloorPosition = position,
            Height = 1,
        };
        if (!GlobalSignals.RoomConstructing(new(this, room)).IsAllowed)
            return;
        RoomIdCounter++;
        Rooms[RoomIdCounter] = room;
        GlobalSignals.RoomConstructed(new(this, room));
    }

    public bool PositionVacant(int elevation, int position)
    {
        if (vacancies is null)
        {
            var rooms = Rooms.Values
                .SelectMany(
                    room => Enumerable
                        .Range(room.FloorPosition, (int)room.Definition.Width)
                        .Select(p => (room.Elevation, p)))
                .ToHashSet();
            var floorSpots = Floors.Values
                .SelectMany(
                    floor => Enumerable
                        .Range(floor.LeftBound, floor.RightBound - floor.LeftBound + 1)
                        .Select(p => (floor.Elevation, p))
                ).ToHashSet();
            vacancies = [.. floorSpots.Except(rooms)];
        }
        return vacancies.Contains((elevation, position));
    }

    #endregion


    #region Floor functions

    public void EnsureGroundFloor()
    {
        if (Floors.Count == 0)
        {
            Floors[0] = new()
            {
                TowerState = this,
                LeftBound = 3,
                RightBound = 3,
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
        var floor = new FloorState()
        {
            TowerState = this,
            LeftBound = 3,
            RightBound = 3,
            Elevation = (int)HighestFloor + 1,
            Definition = DefaultAboveGroundFloorDefinition,
        };
        if (GlobalSignals.FloorConstructing(new(this, floor)).IsAllowed)
        {
            HighestFloor++;
            Floors[(int)HighestFloor] = floor;
            GlobalSignals.FloorConstructed(new(this, floor));
        }
    }

    public void AddBasementFloor()
    {
        EnsureGroundFloor();
        if (LowestFloor >= MaxBasement)
            return;
        var floor = new FloorState()
        {
            TowerState = this,
            LeftBound = 3,
            RightBound = 3,
            Elevation = -(int)(LowestFloor + 1),
            Definition = DefaultBasementFloorDefinition ?? DefaultAboveGroundFloorDefinition,
        };
        if (GlobalSignals.FloorConstructing(new(this, floor)).IsAllowed)
        {
            LowestFloor++;
            Floors[-(int)LowestFloor] = floor;
            GlobalSignals.FloorConstructed(new(this, floor));
        }
    }

    #endregion

    #region operators

    public bool Compare(TowerState other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return RoomIdCounter == other.RoomIdCounter
            && LowestFloor == other.LowestFloor
            && HighestFloor == other.HighestFloor
            && Floors.Keys.SequenceEqual(other.Floors.Keys)
            && Rooms.Keys.SequenceEqual(other.Rooms.Keys)
            && Transports.Keys.SequenceEqual(other.Transports.Keys)
            && Workers.Keys.SequenceEqual(other.Workers.Keys)
            && Wallet == other.Wallet
            && MaxBasement == other.MaxBasement
            && MaxHeight == other.MaxHeight
            && MaxWidth == other.MaxWidth;
    }

    #endregion

    #region Copy/DeSerialize

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
        Wallet = Wallet.Copy(),
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
        { nameof(Wallet), Wallet.Serialize() },
    };

    public TowerState Deserialize(Dictionary<string, Variant> dict)
    {
        Floors = dict[nameof(Floors)].AsSaveFormatDict().DeSerialize<int, FloorState>(int.Parse);
        Rooms = dict[nameof(Rooms)].AsSaveFormatDict().DeSerialize<uint, RoomState>(uint.Parse);
        Transports = dict[nameof(Transports)].AsSaveFormatDict().DeSerialize<uint, TransportState>(uint.Parse);
        Workers = dict[nameof(Workers)].AsSaveFormatDict().DeSerialize<uint, WorkerState>(uint.Parse);
        Wallet.Deserialize(dict[nameof(Wallet)].AsSaveFormatDict());
        RoomIdCounter = dict[nameof(RoomIdCounter)].AsUInt32();
        LowestFloor = dict[nameof(LowestFloor)].AsUInt32();
        HighestFloor = dict[nameof(HighestFloor)].AsUInt32();
        MaxBasement = dict[nameof(MaxBasement)].AsUInt32();
        MaxWidth = dict[nameof(MaxWidth)].AsUInt32();
        MaxHeight = dict[nameof(MaxHeight)].AsUInt32();
        return this;
    }

    #endregion
}
