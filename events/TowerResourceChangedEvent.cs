using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events;

public partial class TowerResourceChangedEvent(TowerState tower, NumericDict<ItemDefinition, uint> amount) : BaseEvent, IDebug, ITowerEvent
{
    public TowerState TowerState { get; } = tower;
    public NumericDict<ItemDefinition, uint> Amount { get; } = amount;
}
