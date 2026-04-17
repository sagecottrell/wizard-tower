using wizardtower.events;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void AddToWallet(TowerResourceChangingEvent @event)
    {
        if (GlobalSignals.TowerResourceChanging(@event).IsAllowed)
        {
            var state = @event.TowerState;
            var cost = @event.Amount;
            state.Wallet.Added(cost);
            GlobalSignals.TowerResourceChanged(new(state, cost) { Source = @event });
        }
    }

    public static void RemoveFromWallet(TowerResourceChangingEvent @event)
    {
        if (GlobalSignals.TowerResourceChanging(@event).IsAllowed)
        {
            var state = @event.TowerState;
            var cost = @event.Amount;
            state.Wallet.Subtracted(cost);
            GlobalSignals.TowerResourceChanged(new(state, cost) { Source = @event });
        }
    }
}
