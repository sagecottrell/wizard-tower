using Godot;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class FloorState : Resource
{
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
    public uint SizeLeft { get; set; }

    [Export]
    public uint SizeRight { get; set; }

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
}
