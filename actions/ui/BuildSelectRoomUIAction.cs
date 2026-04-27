using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void BuildSelectRoom(RoomConstructionSelectingEvent @event)
    {
        var roomDef = @event.RoomDefinition;
        var state = @event.TowerState;
        var ev = GlobalSignals.RoomConstructionSelecting(@event);
        if (state.Wallet >= roomDef.CostToBuildPerUnit && ev.IsAllowed)
            GlobalSignals.RoomConstructionSelected(new(state, roomDef) { Source = @event.Source });
    }
}
