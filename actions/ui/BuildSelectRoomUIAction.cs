using wizardtower.events.handlers;
using wizardtower.events.Room.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void BuildSelectRoom(RoomConstructionSelectingEvent @event)
    {
        var roomDef = @event.RoomDefinition;
        var state = @event.TowerState;
        var ev = RoomEvents.Ui.OnConstructionSelecting(@event);
        if (state.Wallet >= roomDef.CostToBuildPerUnit && ev.IsAllowed)
            RoomEvents.Ui.OnConstructionSelected(new(state, roomDef) { Source = @event.Source });
    }
}
