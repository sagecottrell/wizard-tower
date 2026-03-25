using Godot;
using Godot.Collections;
using System;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class FloorState : Resource, ICopy<FloorState>, IDeSerialize<FloorState>
{
    private uint sizeLeft;
    private uint sizeRight;

    [Signal]
    public delegate void OnSizeChangedEventHandler(FloorState floor);

    public TowerState? TowerState { get; set; }

    public FloorState Copy() => new()
    {
        Definition = Definition,
        Elevation = Elevation,
        SizeLeft = SizeLeft,
        SizeRight = SizeRight,
    };

    [Export]
    public FloorDefinition? Definition { get; set; }

    [Export]
    public int Elevation { get; set; }

    [Export]
    public uint SizeLeft { 
        get => sizeLeft; 
        set
        {
            sizeLeft = Math.Min(MaxWidth, value);
            EmitSignalOnSizeChanged(this);
        }
    }

    [Export]
    public uint SizeRight { 
        get => sizeRight; 
        set {
            sizeRight = Math.Min(MaxWidth, value);
            EmitSignalOnSizeChanged(this);
        }
    }

    [Export]
    public uint MaxWidth { get; set; } = 10;

    public static bool operator ==(FloorState self, FloorState other) => self.Equals(other);

    public static bool operator !=(FloorState self, FloorState other) => !(self == other);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is null) return false;
        if (obj is not FloorState other) return false;
        return Elevation == other.Elevation && SizeLeft == other.SizeLeft && SizeRight == other.SizeRight && Definition == other.Definition;
    }

    public override int GetHashCode() => base.GetHashCode();

    public Dictionary<string, Variant> Serialize() => new()
    {
        { nameof(Definition), Definition?.ResourcePath ?? "" },
        { nameof(Elevation), Elevation },
        { nameof(SizeLeft), SizeLeft },
        { nameof(SizeRight), SizeRight },
        { nameof(MaxWidth), MaxWidth },
    };

    public FloorState Deserialize(Dictionary<string, Variant> dict)
    {
        Definition = LoadDefs.Get<FloorDefinition>(dict[nameof(Definition)].AsString());
        Elevation = dict[nameof(Elevation)].AsInt32();
        sizeLeft = dict[nameof(SizeLeft)].AsUInt32();
        sizeRight = dict[nameof(SizeRight)].AsUInt32();
        MaxWidth = dict[nameof(MaxWidth)].AsUInt32();
        return this;
    }
}
