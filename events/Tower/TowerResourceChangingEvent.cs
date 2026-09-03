/**
Generated from ./events/Tower/TowerResourceChangedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Tower;

public partial class TowerResourceChangingEvent(TowerState tower, NumericDict<ItemDefinition, uint> amount) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public NumericDict<ItemDefinition, uint> Amount { get; set; } = amount;
}


public static class TowerResourceChangedEventExtensions {
    public static TowerResourceChangedEvent Into(this TowerResourceChangingEvent old) {
        return new(tower: old.TowerState, amount: old.Amount) { Source = old, };
    }

    public static TowerResourceChangingEvent TowerResourceChangingEvent(this IEvent ev, TowerState tower, NumericDict<ItemDefinition, uint> amount)
    {
        return new(tower, amount) { Source = ev };
    }
}