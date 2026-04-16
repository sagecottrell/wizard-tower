using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void BuildSelectRoom(RoomConstructionSelectingEvent @event)
    {
        var roomDef = @event.RoomDefinition;
        if (_currentlyBuilding == roomDef)
            return;
        var state = @event.TowerState;
        BuildDeselectForce(state);
        _currentlyBuilding = roomDef;
        var ev = GlobalSignals.RoomConstructionSelecting(new(state, roomDef) { Source = @event });
        if (state.Wallet >= roomDef.CostToBuildPerUnit && ev.IsAllowed)
            GlobalSignals.RoomConstructionSelected(new(state, roomDef) { Source = @event });
    }
}
