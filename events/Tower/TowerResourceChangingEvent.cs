using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Tower;

public partial class TowerResourceChangingEvent(TowerState tower, NumericDict<ItemDefinition, uint> amount) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public NumericDict<ItemDefinition, uint> Amount { get; } = amount;
}
