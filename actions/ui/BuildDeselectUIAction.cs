using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void StopBuildRoom(RoomConstructionStoppingEvent @event)
    {
        if (!GlobalSignals.RoomConstructionStopping(@event).IsAllowed)
            return;

    }
}
