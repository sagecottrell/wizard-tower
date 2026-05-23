using System.Linq;
using wizardtower.events.Floor;
using wizardtower.events.handlers;

namespace wizardtower.actions;

public static class FloorActions
{
    public static void Construct(FloorConstructingEvent @event)
    {
        var tower = @event.TowerState;
        if (FloorEvents.OnConstructing(@event).IsAllowed)
        {
            TowerActions.RemoveFromWallet(new(tower, @event.Floor.Definition.CostToBuildPerUnit * @event.Floor.Width) { Source = @event });
            tower.OnAddFloor(@event.Floor);
            FloorEvents.OnConstructed(new(tower, @event.Floor) { Source = @event.Source });
        }
    }
    public static void Extend(FloorExtendingEvent @event)
    {
        var tower = @event.TowerState;
        if (FloorEvents.OnExtending(@event).IsAllowed)
        {
            TowerActions.RemoveFromWallet(new(tower, @event.Floor.Definition.CostToBuildPerUnit * @event.ExtensionAmount) { Source = @event });
            tower.ExtendFloor(@event.Floor, @event.ExtendedLeft, @event.ExtendedRight);
            FloorEvents.OnExtended(new(tower, @event.Floor, @event.ExtendedLeft, @event.ExtendedRight) { Source = @event.Source });
        }
    }

    public static void Replace(FloorReplacingEvent @event)
    {
        var tower = @event.TowerState;
        if (FloorEvents.OnReplacing(@event).IsAllowed)
        {
            var floor = @event.Floor;
            TowerActions.RemoveFromWallet(new(tower, floor.Definition.CostToBuildPerUnit * floor.Width) { Source = @event });
            floor.Definition = @event.NewDefinition;
            foreach (var room in tower.RoomsOnFloor(floor.Elevation).ToList())
                if (!room.Definition.AllowedFloors.Contains(floor.Definition))
                    RoomActions.Destroy(new(tower, room) { Source = @event });
            FloorEvents.OnReplaced(new(tower, floor, @event.NewDefinition) { Source = @event.Source });
        }
    }
}
