using Godot;

namespace wizardtower.resource_types.production;

[Tool]
[GlobalClass]
public partial class ConsumeWorkers : Resource
{
    [Export]
    public NumericDict<WorkerDefinition, uint> Workers { get; set; } = [];

}
