using wizardtower.events;
using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void Hide(HidingUIEvent @event)
    {
        if (!GlobalSignals.HidingUI(@event).IsAllowed)
            return;
        @event.UserInterface.Hide();
        GlobalSignals.HiddenUI(new HiddenUIEvent(@event.UserInterface).CopySourceInput(@event));
    }
}
