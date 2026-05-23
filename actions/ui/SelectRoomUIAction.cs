using wizardtower.events.handlers;
using wizardtower.events.Room.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static bool SelectRoom(RoomSelectingEvent @event)
    {
        if (!RoomEvents.UI.OnSelecting(@event).IsAllowed)
            return false;
        RoomEvents.UI.OnSelected(new RoomSelectedEvent(@event.TowerState, @event.RoomState) { Source = @event.Source });
        return true;
    }

    public static bool DeselectRoom(RoomDeselectingEvent @event)
    {
        if (!RoomEvents.UI.OnDeselecting(@event).IsAllowed)
            return false;
        RoomEvents.UI.OnDeselected(new RoomDeselectedEvent(@event.TowerState, @event.RoomState) { Source = @event.Source });
        return true;
    }
}
