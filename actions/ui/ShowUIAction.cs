using wizardtower.events.handlers;
using wizardtower.events.Interface;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void ShowUI(InterfaceShowingEvent @event)
    {
        if (!InterfaceEvents.OnShowing(@event).IsAllowed)
            return;
        @event.UserInterface.Show();
        InterfaceEvents.OnShowed(new(@event.UserInterface) { Source = @event.Source });
    }
}
