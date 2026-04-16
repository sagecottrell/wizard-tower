using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void BuildSelectFloor(FloorConstructionSelectingEvent @event)
    {
        var floorDef = @event.FloorDefinition;
        if (floorDef == _currentlyBuilding)
            return;
        var state = @event.TowerState;
        BuildDeselectForce(state);
        _currentlyBuilding = floorDef;
        var ev = GlobalSignals.FloorConstructionSelecting(new(state, floorDef) { Source = @event });
        if (state.Wallet >= floorDef.CostToBuildPerUnit && ev.IsAllowed)
            GlobalSignals.FloorConstructionSelected(new(state, floorDef) { Source = @event });
    }
}
