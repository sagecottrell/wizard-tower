using Godot;
using Godot.Collections;
using System;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class FloorState : Resource, ICopy<FloorState>, IDeSerialize<FloorState>
{
    private int leftBound;
    private int rightBound;

    [Signal]
    public delegate void OnSizeChangedEventHandler(FloorState floor);

    public TowerState? TowerState { get; set; }

    public FloorState Copy() => new()
    {
        Definition = Definition,
        Elevation = Elevation,
        LeftBound = LeftBound,
        RightBound = RightBound,
    };

    [Export]
    public FloorDefinition Definition { get; set; } = new();

    [Export]
    public int Elevation { get; set; }

    [Export]
    public int LeftBound { 
        get => leftBound; 
        set
        {
            leftBound = Math.Max(value, -(int)MaxWidth);
            EmitSignalOnSizeChanged(this);
        }
    }

    [Export]
    public int RightBound { 
        get => rightBound; 
        set {
            rightBound = Math.Min(value, (int)MaxWidth);
            EmitSignalOnSizeChanged(this);
        }
    }

    public uint Width => (uint)(RightBound - LeftBound + 1);

    [Export]
    public uint MaxWidth { get; set; } = 10;

    public bool Compare(FloorState? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return Elevation == other.Elevation 
            && LeftBound == other.LeftBound 
            && RightBound == other.RightBound 
            && Definition == other.Definition;
    }

    public Dictionary<string, Variant> Serialize() => new()
    {
        { nameof(Definition), Definition?.ResourcePath ?? "" },
        { nameof(Elevation), Elevation },
        { nameof(LeftBound), LeftBound },
        { nameof(RightBound), RightBound },
        { nameof(MaxWidth), MaxWidth },
    };

    public FloorState Deserialize(Dictionary<string, Variant> dict)
    {
        Definition = LoadDefs.Get<FloorDefinition>(dict[nameof(Definition)].AsString()) ?? Definition;
        Elevation = dict[nameof(Elevation)].AsInt32();
        leftBound = dict[nameof(LeftBound)].AsInt32();
        rightBound = dict[nameof(RightBound)].AsInt32();
        MaxWidth = dict[nameof(MaxWidth)].AsUInt32();
        return this;
    }
}
