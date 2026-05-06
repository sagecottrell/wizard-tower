using wizardtower.events;
using wizardtower.events.handlers;
using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void Hide(HidingUIEvent @event)
    {
        if (!GeneralEvents.OnHidingUI(@event).IsAllowed)
            return;
        @event.UserInterface.Hide();
        GeneralEvents.OnHiddenUI(new HiddenUIEvent(@event.UserInterface).CopySourceInput(@event));
    }
}
