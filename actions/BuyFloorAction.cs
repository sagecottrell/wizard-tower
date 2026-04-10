using wizardtower.events;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void BuyFloor(FloorConstructingEvent @event)
    {
        var tower = @event.TowerState;
        if (GlobalSignals.FloorConstructing(@event).IsAllowed)
        {
            RemoveFromWallet(tower, @event.Floor.Definition.CostToBuildPerUnit * @event.Floor.Width, @event);
            tower.OnAddFloor(@event.Floor);
            GlobalSignals.FloorConstructed(new(tower, @event.Floor));
        }
    }
}
