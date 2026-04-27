using wizardtower.events;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void BuyFloor(FloorConstructingEvent @event)
    {
        var tower = @event.TowerState;
        if (GlobalSignals.FloorConstructing(@event).IsAllowed)
        {
            RemoveFromWallet(new(tower, @event.Floor.Definition.CostToBuildPerUnit * @event.Floor.Width) { Source = @event });
            tower.OnAddFloor(@event.Floor);
            GlobalSignals.FloorConstructed(new(tower, @event.Floor) { Source = @event.Source });
        }
    }
}
