using wizardtower.events.handlers;
using wizardtower.events.Room.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static bool SelectRoom(RoomSelectingEvent @event)
    {
        if (!RoomEvents.Ui.OnSelecting(@event).IsAllowed)
            return false;
        RoomEvents.Ui.OnSelected(new RoomSelectedEvent(@event.TowerState, @event.RoomState) { Source = @event.Source });
        return true;
    }

    public static bool DeselectRoom(RoomDeselectingEvent @event)
    {
        if (!RoomEvents.Ui.OnDeselecting(@event).IsAllowed)
            return false;
        RoomEvents.Ui.OnDeselected(new RoomDeselectedEvent(@event.TowerState, @event.RoomState) { Source = @event.Source });
        return true;
    }
}
