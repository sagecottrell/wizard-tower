using Godot;

namespace wizardtower.resource_types.production;

[Tool]
[GlobalClass]
internal partial class ProduceWorkers : BaseProductionInfo
{
    [Export]
    public NumericDict<WorkerDefinition, uint> WorkersToProduce { get; set; } = [];
}
