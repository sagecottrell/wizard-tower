using Godot;
using Godot.Collections;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class TransportState : Resource, ICopy<TransportState>, IDeSerialize<TransportState>
{
    [Export]
    public TransportDefinition Definition { get; set; } = new();

    [Export]
    public int Elevation { get; set; }

    [Export]
    public int HorizontalPosition { get; set; }

    [Export]
    public uint Height { get; set; } = 1;

    [Export]
    public Array<int> LoadStatistics { get; set; }

    public bool Compare(TransportState? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return Elevation == other.Elevation && HorizontalPosition == other.HorizontalPosition && Height == other.Height && Definition == other.Definition;
    }

    public TransportState Copy() => new()
    {
        Definition = Definition,
    };

    public Godot.Collections.Dictionary<string, Variant> Serialize() => [];
    public TransportState Deserialize(Godot.Collections.Dictionary<string, Variant> dict) => this;
}
