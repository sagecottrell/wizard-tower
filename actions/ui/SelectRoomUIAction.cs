using wizardtower.events.handlers;
using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static bool SelectRoom(RoomSelectingEvent @event)
    {
        if (!RoomEvents.UI.OnRoomSelecting(@event).IsAllowed)
            return false;
        RoomEvents.UI.OnRoomSelected(new RoomSelectedEvent(@event.TowerState, @event.Room) { Source = @event.Source });
        return true;
    }

    public static bool DeselectRoom(RoomDeselectingEvent @event)
    {
        if (!RoomEvents.UI.OnRoomDeselecting(@event).IsAllowed)
            return false;
        RoomEvents.UI.OnRoomDeselected(new RoomDeselectedEvent(@event.TowerState, @event.Room) { Source = @event.Source });
        return true;
    }
}
