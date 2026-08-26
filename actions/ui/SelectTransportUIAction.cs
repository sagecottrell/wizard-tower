using wizardtower.events.handlers;
using wizardtower.events.Transport.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static bool SelectTransport(TransportSelectingEvent @event)
    {
        if (!TransportEvents.Ui.OnSelecting(@event).IsAllowed)
            return false;
        TransportEvents.Ui.OnSelected(new TransportSelectedEvent(@event.TowerState, @event.TransportState) { Source = @event.Source });
        return true;
    }

    public static bool DeselectTransport(TransportDeselectingEvent @event)
    {
        if (!TransportEvents.Ui.OnDeselecting(@event).IsAllowed)
            return false;
        TransportEvents.Ui.OnDeselected(new TransportDeselectedEvent(@event.TowerState, @event.TransportState) { Source = @event.Source });
        return true;
    }
}
