using Godot;
using Godot.Collections;

namespace wizardtower.custom_godot_resources.production;

internal partial class ProduceWorkers : BaseProductionInfo
{
    [Export]
    public Array<WorkerDefinition> WorkersToProduce { get; set; } = [];
}
