using wizardtower.events;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void BuyTransport(TransportConstructingEvent @event)
    {
        var tower = @event.TowerState;
        if (GlobalSignals.TransportConstructing(@event).IsAllowed)
        {
            RemoveFromWallet(new(tower, @event.Transport.Definition.CostToBuild) { Source = @event });
            tower.AddTransport(@event.Transport);
            GlobalSignals.TransportConstructed(new(tower, @event.Transport) { Source = @event });
        }
    }
}
