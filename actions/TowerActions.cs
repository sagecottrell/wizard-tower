using wizardtower.events.handlers;
using wizardtower.events.Tower;

namespace wizardtower.actions;

public static class TowerActions
{
    public static void AddToWallet(TowerResourceChangingEvent @event)
    {
        if (TowerEvents.OnResourceChanging(@event).IsAllowed)
        {
            var state = @event.TowerState;
            var cost = @event.Amount;
            state.Wallet.Added(cost);
            TowerEvents.OnResourceChanged(new(state, cost) { Source = @event });
        }
    }

    public static void RemoveFromWallet(TowerResourceChangingEvent @event)
    {
        if (TowerEvents.OnResourceChanging(@event).IsAllowed)
        {
            var state = @event.TowerState;
            var cost = @event.Amount;
            state.Wallet.Subtracted(cost);
            TowerEvents.OnResourceChanged(new(state, cost) { Source = @event });
        }
    }
}
