using Godot;
using System.Collections.Generic;
using System.Linq;
using wizardtower.resource_types;
using wizardtower.UIs.editor_add_room;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class TowerState : Resource, ICopy<TowerState>, IDeSerialize<TowerState>, IDebug
{
    private uint maxWidth = 10;

    private HashSet<(int elevation, int position)>? vacancies;
    private Dictionary<int, List<RoomState>>? roomsByFloor;
    private Dictionary<int, List<TransportState>>? transportsByFloor;

    [Export]
    public string Name { get; set; } = "Tower";

    [Export]
    public Godot.Collections.Dictionary<int, FloorState> Floors { get; set; } = [];

    [ExportToolButton("Add Top Floor")]
    public Callable addTopFloor => Callable.From(_editorAddTopFloor);

    [ExportToolButton("Add Basement Floor")]
    public Callable addBopFloor => Callable.From(_editorAddBopFloor);

    [Export]
    public Godot.Collections.Dictionary<uint, RoomState> Rooms { get; set; } = [];

    private Dictionary<int, List<RoomState>> RoomsByFloor { 
        get
        {
            if (roomsByFloor is null)
            {
                roomsByFloor = [];
                foreach (var room in Rooms.Values)
                    _addRoomByFloor(room);
            }
            return roomsByFloor;
        }
    }

    [ExportToolButton("Add New Room")]
    public Callable addroom => Callable.From(EditorAddRoom);

    [Export]
    public Godot.Collections.Dictionary<uint, WorkerState> Workers { get; set; } = [];

    [Export]
    public Godot.Collections.Dictionary<uint, TransportState> Transports { get; set; } = [];

    private Dictionary<int, List<TransportState>> TransportsByFloor
    {
        get
        {
            if (transportsByFloor is null)
            {
                transportsByFloor = [];
                foreach (var transport in Transports.Values)
                    _addTransportByFloor(transport);
            }
            return transportsByFloor;
        }
    }

    [Export]
    public FloorDefinition DefaultAboveGroundFloorDefinition { get; set; } = new();

    [Export]
    public FloorDefinition DefaultBasementFloorDefinition { get; set; } = new();

    [Export]
    public NumericDict<ItemDefinition, uint> Wallet { get; set; } = [];

    [Export]
    public Godot.Collections.Array<RoomDefinition> UnlockedRooms { get; set; } = [];

    [Export]
    public Godot.Collections.Array<FloorDefinition> UnlockedFloors { get; set; } = [];

    [Export]
    public Godot.Collections.Array<TransportDefinition> UnlockedTransports { get; set; } = [];

    [Export]
    public int DefaultFloorLeftBound { get; set; } = -3;

    [Export]
    public int DefaultFloorRightBound { get; set; } = 3;

    [Export]
    public uint RoomIdCounter { get; set; } = 0;

    [Export]
    public uint TransportIdCounter { get; set; } = 0;

    [Export]
    public int LowestFloor { get; set; } = 0;

    [Export]
    public int HighestFloor { get; set; } = 0;

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
                AddRoom(e, p, r);
            };
        }
        vacancies = null;
    }

    public void AddRoom(int elevation, int position, RoomDefinition r) => AddRoom(new RoomState()
    {
        Definition = r,
        Elevation = elevation,
        FloorPosition = position,
        Height = 1,
    });

    public void AddRoom(RoomState room)
    {
        RoomIdCounter++;
        room.Id = RoomIdCounter;
        Rooms[RoomIdCounter] = room;
        vacancies?.ExceptWith(_roomRange(room));
        _addRoomByFloor(room);
    }

    public void RemoveRoom(RoomState room)
    {
        vacancies?.UnionWith(_roomRange(room));
        Rooms.Remove(room.Id);
        if (RoomsByFloor is not null && RoomsByFloor.TryGetValue(room.Elevation, out var floorList))
            floorList.Remove(room);
    }

    private void _addRoomByFloor(RoomState room)
    {
        if (!RoomsByFloor.TryGetValue(room.Elevation, out var floorList))
        {
            floorList = [];
            RoomsByFloor[room.Elevation] = floorList;
        }
        floorList.Add(room);
    }

    #endregion

    #region Vacancies

    public bool PositionVacant(int elevation, int position)
    {
        if (vacancies is null)
        {
            var rooms = Rooms.Values.SelectMany(_roomRange).ToHashSet();
            var transports = Transports.Values.SelectMany(_transportRange).ToHashSet();
            var floorSpots = Floors.Values.SelectMany(_floorRange).ToHashSet();
            vacancies = [.. floorSpots.Except(rooms).Except(transports)];
        }
        return vacancies.Contains((elevation, position));
    }

    private static IEnumerable<(int elevation, int position)> _roomRange(RoomState room)
        => Enumerable.Range(room.FloorPosition, (int)room.Definition.Width)
            .SelectMany(p => Enumerable.Range(room.Elevation, (int)room.Height).Select(e => (e, p)));
    private static IEnumerable<(int elevation, int position)> _transportRange(TransportState transport)
        => Enumerable.Range(transport.HorizontalPosition, (int)transport.Definition.Width)
            .SelectMany(p => Enumerable.Range(transport.Elevation, (int)transport.Height).Select(e => (e, p)));
    private static IEnumerable<(int elevation, int position)> _floorRange(FloorState floor)
        => Enumerable.Range(floor.LeftBound, floor.RightBound - floor.LeftBound + 1).Select(p => (floor.Elevation, p));

    #endregion

    #region Floor functions

    public bool IsHeightLimitReached => HighestFloor >= MaxHeight;
    public bool IsDepthLimitReached => -LowestFloor >= MaxBasement;

    public void EnsureGroundFloor()
    {
        if (Floors.Count == 0)
        {
            var floor = new FloorState()
            {
                TowerState = this,
                LeftBound = DefaultFloorLeftBound,
                RightBound = DefaultFloorRightBound,
                Elevation = 0,
                Definition = DefaultAboveGroundFloorDefinition,
            };
            Floors[0] = floor;
            vacancies?.UnionWith(_floorRange(floor));
        }
    }

    private void _editorAddTopFloor()
    {
        if (IsHeightLimitReached)
            return;
        EnsureGroundFloor();
        var floor = NewTopFloor();
        OnAddFloor(floor);
    }

    private void _editorAddBopFloor()
    {
        if (IsHeightLimitReached)
            return;
        EnsureGroundFloor();
        var floor = NewBasementFloor();
        OnAddFloor(floor);
    }

    /// <summary>
    /// Creates a floor that would be above the top floor, does not actually add the floor to the tower
    /// </summary>
    /// <returns></returns>
    public FloorState NewTopFloor(FloorDefinition? def = null) => new()
    {
        TowerState = this,
        LeftBound = DefaultFloorLeftBound,
        RightBound = DefaultFloorRightBound,
        Elevation = HighestFloor + 1,
        Definition = def ?? DefaultAboveGroundFloorDefinition,
    };

    public FloorState NewBasementFloor(FloorDefinition? def = null) => new()
    {
        TowerState = this,
        LeftBound = DefaultFloorLeftBound,
        RightBound = DefaultFloorRightBound,
        Elevation = LowestFloor - 1,
        Definition = def ?? DefaultBasementFloorDefinition ?? DefaultAboveGroundFloorDefinition,
    };

    public void OnAddFloor(FloorState floor)
    {
        Floors[floor.Elevation] = floor;
        if (floor.Elevation > HighestFloor)
            HighestFloor = floor.Elevation;
        if (floor.Elevation < LowestFloor)
            LowestFloor = floor.Elevation;
        vacancies?.UnionWith(_floorRange(floor));
    }

    public void ExtendFloor(FloorState floor, uint left, uint right)
    {
        if (left > 0)
            vacancies?.UnionWith(Enumerable.Range(floor.LeftBound - (int)left, (int)left).Select(i => (floor.Elevation, i)));
        if (right > 0)
            vacancies?.UnionWith(Enumerable.Range(floor.RightBound + 1, (int)right).Select(i => (floor.Elevation, i)));
        this.Debug($"Extending floor at elevation {floor.Elevation} from bounds ({floor.LeftBound}, {floor.RightBound}) to ({floor.LeftBound - (int)left}, {floor.RightBound + (int)right})");
        floor.LeftBound -= (int)left;
        floor.RightBound += (int)right;
    }

    public IReadOnlyList<RoomState> RoomsOnFloor(int elevation) => RoomsByFloor.TryGetValue(elevation, out var list) ? list : [];

    #endregion

    #region Transport functions

    public void AddTransport(TransportState transport)
    {
        TransportIdCounter++;
        transport.Id = TransportIdCounter;
        Transports[TransportIdCounter] = transport;
        vacancies?.ExceptWith(_transportRange(transport));
        _addTransportByFloor(transport);
    }

    private void _addTransportByFloor(TransportState transport)
    {
        if (!TransportsByFloor.TryGetValue(transport.Elevation, out var floorList))
        {
            floorList = [];
            for (var e = transport.Elevation; e < transport.Elevation + transport.Height; e++)
                TransportsByFloor[e] = floorList;
        }
        floorList.Add(transport);
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
        LowestFloor = dict[nameof(LowestFloor)].AsInt32();
        HighestFloor = dict[nameof(HighestFloor)].AsInt32();
        MaxBasement = dict[nameof(MaxBasement)].AsUInt32();
        MaxWidth = dict[nameof(MaxWidth)].AsUInt32();
        MaxHeight = dict[nameof(MaxHeight)].AsUInt32();
        return this;
    }

    #endregion
}
