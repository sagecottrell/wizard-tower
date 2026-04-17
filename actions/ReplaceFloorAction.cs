using System.Linq;
using wizardtower.events;

namespace wizardtower.actions;

public static partial class Actions
{
    public static void ReplaceFloor(FloorReplacingEvent @event)
    {
        var tower = @event.TowerState;
        if (GlobalSignals.FloorReplacing(@event).IsAllowed)
        {
            var floor = @event.Floor;
            RemoveFromWallet(new(tower, floor.Definition.CostToBuildPerUnit * floor.Width) { Source = @event });
            floor.Definition = @event.NewDefinition;
            foreach (var room in tower.RoomsOnFloor(floor.Elevation).ToList())
                if (!room.Definition.AllowedFloors.Contains(floor.Definition))
                    DestroyRoom(new(tower, room) { Source = @event });
            GlobalSignals.FloorReplaced(new(tower, floor, @event.NewDefinition) { Source = @event });
        }
    }
}
