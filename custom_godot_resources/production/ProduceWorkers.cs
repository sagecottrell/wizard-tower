using Godot;

namespace wizardtower.custom_godot_resources.production;

[Tool]
[GlobalClass]
internal partial class ProduceWorkers : BaseProductionInfo
{
    [Export]
    public NumericDict<WorkerDefinition, uint> WorkersToProduce { get; set; } = [];
}
