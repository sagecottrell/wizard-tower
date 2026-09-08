using System.Linq;
using Godot;
using Godot.Collections;
using wizardtower.resource_types;
using wizardtower.state.room_functions;

namespace wizardtower.state;

[Tool]
[GlobalClass]
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
    public ItemDefinition? Warehouse { get; set; }

    [Export]
    public NumericDict<ItemDefinition, uint> StoredItems { get; set; } = [];

    [Export]
    public NumericDict<WorkerDefinition, uint> StoredWorkers { get; set; } = [];

    [Export]
    public Array<RoomStateWorkerPath> WorkerPaths { get; set; } = [];

    [Export]
    public RoomConvertResourcesState? ConvertResourcesState { get; set; }

    /// <summary>
    /// rooms whos configurations are linked.
    /// if one is changed, the user should be prompted with confirmation to automatically update the others
    /// </summary>
    [Export]
    public Array<uint> LinkedRooms { get; set; } = [];

    /// <summary>
    /// useful for game logic as well as the UI to display status
    /// </summary>
    /// <returns></returns>
    public bool HasSufficientWorkers() => Definition.ResourceConversion?.WorkerKind is null
        || (StoredWorkers.TryGetValue(Definition.ResourceConversion.WorkerKind, out var workersPresent) && workersPresent >= Definition.ResourceConversion.WorkersCount);

    /// <summary>
    /// useful for game logic as well as the UI to display status
    /// </summary>
    /// <returns></returns>
    public bool HasSufficientMaterials() => ConvertResourcesState?.SelectedRecipe?.Input is null || StoredItems >= ConvertResourcesState.SelectedRecipe.Input;

    public NumericDict<ItemDefinition, float>? InputRate => ConvertResourcesState?.SelectedRecipe is { } r && r?.Input is { } i && Definition.ResourceConversion?.ProcessingTimeMultiplier is float m
        ? i.Convert(x => x / m / r.ProcessingTimeSeconds) : null;
    public NumericDict<ItemDefinition, float>? OutputRate => ConvertResourcesState?.SelectedRecipe is { } r && Definition.ResourceConversion?.ProcessingTimeMultiplier is float m
        ? r.AverageItemOutputRate?.MultipliedByScalar(1 / m) : null;


    public System.Collections.Generic.HashSet<ItemDefinition>? RelatedItems => PossibleOutputs != null && Inputs != null
        ? [..Inputs.Union(PossibleOutputs)]
        : Inputs ?? PossibleOutputs;

    public System.Collections.Generic.HashSet<ItemDefinition>? PossibleOutputs => [..ConvertResourcesState?.SelectedRecipe?.PossibleOutputs ?? []];

    public System.Collections.Generic.HashSet<ItemDefinition>? Inputs => [.. ConvertResourcesState?.SelectedRecipe?.Input?.Keys ?? []];

    public Vector3 Vec3Position(float z = 0, Vector3 offset = default) => new Vector3(FloorPosition, Elevation, z) + offset;

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