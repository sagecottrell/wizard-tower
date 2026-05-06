using wizardtower.events;
using wizardtower.events.handlers;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void BuyTransport(TransportConstructingEvent @event)
    {
        var tower = @event.TowerState;
        if (TransportEvents.OnTransportConstructing(@event).IsAllowed)
        {
            RemoveFromWallet(new(tower, @event.Transport.Definition.CostToBuild) { Source = @event });
            tower.AddTransport(@event.Transport);
            TransportEvents.OnTransportConstructed(new(tower, @event.Transport) { Source = @event.Source });
        }
    }
}
