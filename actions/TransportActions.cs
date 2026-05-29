using wizardtower.events.handlers;
using wizardtower.events.Transport;

namespace wizardtower.actions;

public static class TransportActions
{
    public static void Construct(TransportConstructingEvent @event)
    {
        var tower = @event.TowerState;
        if (!TransportEvents.OnConstructing(@event).IsAllowed)
            return;
        TowerActions.RemoveFromWallet(new(tower, @event.TransportState.Definition.CostToBuild) { Source = @event });
        tower.AddTransport(@event.TransportState);
        TransportEvents.OnConstructed(new(tower, @event.TransportState) { Source = @event.Source });
    }
}
