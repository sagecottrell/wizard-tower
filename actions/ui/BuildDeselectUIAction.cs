using wizardtower.events.handlers;
using wizardtower.events.Room.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void StopBuildRoom(RoomConstructionStoppingEvent @event)
    {
        if (!RoomEvents.Ui.OnConstructionStopping(@event).IsAllowed)
            return;

    }
}
