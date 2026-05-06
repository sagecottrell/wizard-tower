using wizardtower.events.handlers;
using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void ShowUI(ShowingUIEvent @event)
    {
        if (!GeneralEvents.OnShowingUI(@event).IsAllowed)
            return;
        @event.UserInterface.Show();
        GeneralEvents.OnShowedUI(new(@event.UserInterface) { Source = @event.Source });
    }
}
