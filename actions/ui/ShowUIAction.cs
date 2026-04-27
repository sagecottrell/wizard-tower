using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void ShowUI(ShowingUIEvent @event)
    {
        if (!GlobalSignals.ShowingUI(@event).IsAllowed)
            return;
        @event.UserInterface.Show();
        GlobalSignals.ShowedUI(new(@event.UserInterface) { Source = @event.Source });
    }
}
