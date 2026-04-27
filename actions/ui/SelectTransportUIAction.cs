using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static bool SelectTransport(TransportSelectingEvent @event)
    {
        if (!GlobalSignals.TransportSelecting(@event).IsAllowed)
            return false;
        GlobalSignals.TransportSelected(new TransportSelectedEvent(@event.TowerState, @event.Transport) { Source = @event.Source });
        return true;
    }

    public static bool DeselectTransport(TransportDeselectingEvent @event)
    {
        if (!GlobalSignals.TransportDeselecting(@event).IsAllowed)
            return false;
        GlobalSignals.TransportDeselected(new TransportDeselectedEvent(@event.TowerState, @event.Transport) { Source = @event.Source });
        return true;
    }
}
