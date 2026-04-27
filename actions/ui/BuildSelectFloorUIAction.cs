using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void BuildSelectFloor(FloorConstructionSelectingEvent @event)
    {
        var floorDef = @event.FloorDefinition;
        var state = @event.TowerState;
        var ev = GlobalSignals.FloorConstructionSelecting(@event);
        if (state.Wallet >= floorDef.CostToBuildPerUnit && ev.IsAllowed)
            GlobalSignals.FloorConstructionSelected(new(state, floorDef) { Source = @event });
    }
}
