using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void BuildSelectFloor(TowerState state, FloorDefinition floorDef)
    {
        var c = _currentlyBuilding;
        BuildDeselectForce(state);
        if (floorDef == c)
            return;
        _currentlyBuilding = floorDef;
        var ev = GlobalSignals.FloorConstructionSelecting(new(state, floorDef));
        if (state.Wallet >= floorDef.CostToBuildPerUnit && ev.IsAllowed)
            GlobalSignals.FloorConstructionSelected(new(state, floorDef));
    }
}
