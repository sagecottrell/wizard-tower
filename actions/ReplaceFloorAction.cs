using System.Linq;
using wizardtower.events.Floor;
using wizardtower.events.handlers;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void ReplaceFloor(FloorReplacingEvent @event)
    {
        var tower = @event.TowerState;
        if (FloorEvents.OnFloorReplacing(@event).IsAllowed)
        {
            var floor = @event.Floor;
            RemoveFromWallet(new(tower, floor.Definition.CostToBuildPerUnit * floor.Width) { Source = @event });
            floor.Definition = @event.NewDefinition;
            foreach (var room in tower.RoomsOnFloor(floor.Elevation).ToList())
                if (!room.Definition.AllowedFloors.Contains(floor.Definition))
                    DestroyRoom(new(tower, room) { Source = @event });
            FloorEvents.OnFloorReplaced(new(tower, floor, @event.NewDefinition) { Source = @event.Source });
        }
    }
}
