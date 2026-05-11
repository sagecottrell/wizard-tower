using wizardtower.events.handlers;
using wizardtower.events.Tower;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void AddToWallet(TowerResourceChangingEvent @event)
    {
        if (TowerEvents.OnTowerResourceChanging(@event).IsAllowed)
        {
            var state = @event.TowerState;
            var cost = @event.Amount;
            state.Wallet.Added(cost);
            TowerEvents.OnTowerResourceChanged(new(state, cost) { Source = @event });
        }
    }

    public static void RemoveFromWallet(TowerResourceChangingEvent @event)
    {
        if (TowerEvents.OnTowerResourceChanging(@event).IsAllowed)
        {
            var state = @event.TowerState;
            var cost = @event.Amount;
            state.Wallet.Subtracted(cost);
            TowerEvents.OnTowerResourceChanged(new(state, cost) { Source = @event });
        }
    }
}
