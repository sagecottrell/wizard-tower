using wizardtower.events.Floor.ui;
using wizardtower.events.handlers;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void BuildSelectFloor(FloorConstructionSelectingEvent @event)
    {
        var floorDef = @event.FloorDefinition;
        var state = @event.TowerState;
        var ev = FloorEvents.UI.OnConstructionSelecting(@event);
        if (state.Wallet >= floorDef.CostToBuildPerUnit && ev.IsAllowed)
            FloorEvents.UI.OnConstructionSelected(new(state, floorDef) { Source = @event });
    }
}
