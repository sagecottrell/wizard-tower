using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class TowerResourceChangingEvent(TowerState tower, NumericDict<ItemDefinition, uint> amount, IEvent? cause) : GodotObject, IAllowableEvent, IDebug, ITowerEvent, IEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public NumericDict<ItemDefinition, uint> Amount { get; } = amount;
    public IEvent? Cause { get; } = cause;
}
