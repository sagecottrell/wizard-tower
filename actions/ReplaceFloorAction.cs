using wizardtower.events;
using wizardtower.state;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void ReplaceFloor(TowerState state, FloorReplacingEvent @event)
    {
        if (GlobalSignals.FloorReplacing(@event).IsAllowed)
        {
            var floor = @event.Floor;
            RemoveFromWallet(state, floor.Definition.CostToBuildPerUnit * floor.Width, @event);
            floor.Definition = @event.NewDefinition;
            GlobalSignals.FloorReplaced(new(state, floor, @event.NewDefinition));
        }
    }
}
