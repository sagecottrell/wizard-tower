using Godot;
using Godot.Collections;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class WorkerState : Resource, ICopy<WorkerState>, IDeSerialize<WorkerState>
{
    [Export]
    public WorkerDefinition? WorkerDefinition { get; set; }

    [Export]
    public uint AssignedRoomId { get; set; }

    [Export]
    public uint SourceRoomId { get; set; }

    [Export]
    public bool WalkingAbout { get; set; }

    [Export]
    public int Elevation { get; set; }

    [Export]
    public int FloorPosition { get; set; }

    [Export]
    public ItemDefinition? PayloadKind { get; set; }

    [Export]
    public uint PayloadAmount { get; set; }

    public WorkerState Copy() => new();

    public WorkerState Deserialize(Dictionary<string, Variant> dict)
    {
        return this;
    }

    public Dictionary<string, Variant> Serialize() => [];
}
