using Godot;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    private static Resource? _currentlyBuilding;

    public static void BuildSelectRoom(TowerState state, RoomDefinition roomDef)
    {
        var c  = _currentlyBuilding;
        BuildDeselectForce(state);
        if (c == roomDef)
            return;
        _currentlyBuilding = roomDef;
        var ev = GlobalSignals.RoomConstructionSelecting(new(state, roomDef));
        if (state.Wallet >= roomDef.CostToBuildPerUnit && ev.IsAllowed)
            GlobalSignals.RoomConstructionSelected(new(state, roomDef));
    }
}
