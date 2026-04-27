using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static bool SelectRoom(RoomSelectingEvent @event)
    {
        if (!GlobalSignals.RoomSelecting(@event).IsAllowed)
            return false;
        GlobalSignals.RoomSelected(new RoomSelectedEvent(@event.TowerState, @event.Room) { Source = @event.Source });
        return true;
    }

    public static bool DeselectRoom(RoomDeselectingEvent @event)
    {
        if (!GlobalSignals.RoomDeselecting(@event).IsAllowed)
            return false;
        GlobalSignals.RoomDeselected(new RoomDeselectedEvent(@event.TowerState, @event.Room) { Source = @event.Source });
        return true;
    }
}
