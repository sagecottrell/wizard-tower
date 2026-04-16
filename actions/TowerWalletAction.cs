using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void AddToWallet(TowerState state, NumericDict<ItemDefinition, uint> cost, IEvent @event)
    {
        if (GlobalSignals.TowerResourceChanging(new(state, cost) { Source = @event }).IsAllowed)
        {
            state.Wallet.Added(cost);
            GlobalSignals.TowerResourceChanged(new(state, cost) { Source = @event });
        }
    }

    public static void RemoveFromWallet(TowerState state, NumericDict<ItemDefinition, uint> cost, IEvent @event)
    {
        if (GlobalSignals.TowerResourceChanging(new(state, cost) { Source = @event }).IsAllowed)
        {
            state.Wallet.Subtracted(cost);
            GlobalSignals.TowerResourceChanged(new(state, cost) { Source = @event });
        }
    }
}
