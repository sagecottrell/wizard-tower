using wizardtower.events;
using wizardtower.events.handlers;
using wizardtower.events.Interface;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void Hide(InterfaceHidingEvent @event)
    {
        if (!InterfaceEvents.OnHiding(@event).IsAllowed)
            return;
        @event.UserInterface.Hide();
        InterfaceEvents.OnHided(new InterfaceHidedEvent(@event.UserInterface).CopySourceInput(@event));
    }
}
