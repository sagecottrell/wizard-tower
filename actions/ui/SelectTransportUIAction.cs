using wizardtower.events.handlers;
using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static bool SelectTransport(TransportSelectingEvent @event)
    {
        if (!TransportEvents.UI.OnTransportSelecting(@event).IsAllowed)
            return false;
        TransportEvents.UI.OnTransportSelected(new TransportSelectedEvent(@event.TowerState, @event.Transport) { Source = @event.Source });
        return true;
    }

    public static bool DeselectTransport(TransportDeselectingEvent @event)
    {
        if (!TransportEvents.UI.OnTransportDeselecting(@event).IsAllowed)
            return false;
        TransportEvents.UI.OnTransportDeselected(new TransportDeselectedEvent(@event.TowerState, @event.Transport) { Source = @event.Source });
        return true;
    }
}
