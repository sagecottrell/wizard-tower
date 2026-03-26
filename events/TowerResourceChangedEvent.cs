using Godot;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class TowerResourceChangedEvent(TowerState tower, NumericDict<ItemDefinition, uint> amount) : GodotObject, IDebug
{
    public TowerState Tower { get; } = tower;
    public NumericDict<ItemDefinition, uint> Amount { get; } = amount;
}
