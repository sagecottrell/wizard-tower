using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class TowerResourceChangingEvent(TowerState tower, NumericDict<ItemDefinition, uint> amount) : GodotObject, IAllowableEvent, IDebug
{
    public bool IsAllowed { get; set; }
    public TowerState Tower { get; } = tower;
    public NumericDict<ItemDefinition, uint> Amount { get; } = amount;
}
