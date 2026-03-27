using Godot;
using Godot.Collections;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class WorkerState : Resource, ICopy<WorkerState>, IDeSerialize<WorkerState>
{
    public WorkerState Copy() => new();

    public WorkerState Deserialize(Dictionary<string, Variant> dict)
    {
        return this;
    }

    public Dictionary<string, Variant> Serialize() => [];
}
