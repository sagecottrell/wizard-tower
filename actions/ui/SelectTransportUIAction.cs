using wizardtower.events.handlers;
using wizardtower.events.Transport.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static bool SelectTransport(TransportSelectingEvent @event)
    {
        if (!TransportEvents.UI.OnSelecting(@event).IsAllowed)
            return false;
        TransportEvents.UI.OnSelected(new TransportSelectedEvent(@event.TowerState, @event.TransportState) { Source = @event.Source });
        return true;
    }

    public static bool DeselectTransport(TransportDeselectingEvent @event)
    {
        if (!TransportEvents.UI.OnDeselecting(@event).IsAllowed)
            return false;
        TransportEvents.UI.OnDeselected(new TransportDeselectedEvent(@event.TowerState, @event.TransportState) { Source = @event.Source });
        return true;
    }
}
