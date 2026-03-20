using Godot;

namespace wizardtower.resource_types.production;

[Tool]
[GlobalClass]
internal partial class ProduceResources : BaseProductionInfo
{
    [Export]
    public NumericDict<ItemDefinition, uint> Output { get; set; } = [];

    [Export]
    public uint MaxTimesPerDay { get; set; } = 0;

    [Export]
    public uint ProductionTime { get; set; } = 1;
}
