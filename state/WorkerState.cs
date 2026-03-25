using Godot;
using Godot.Collections;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class WorkerState : Resource, ICopy<WorkerState>, IDeSerialize<WorkerState>
{
    public WorkerState Copy()
    {
        throw new System.NotImplementedException();
    }

    public WorkerState Deserialize(Dictionary<string, Variant> dict)
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, Variant> Serialize()
    {
        throw new System.NotImplementedException();
    }
}
