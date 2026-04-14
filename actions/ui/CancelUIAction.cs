using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void Cancel(CancelledUIEvent @event)
    {
        GlobalSignals.CancelledUI(@event);
    }
}
