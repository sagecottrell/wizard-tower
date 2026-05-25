using wizardtower.events.Game;
using wizardtower.events.handlers;

namespace wizardtower.actions;

public static class GameActions
{
    public static void SetPause(TogglingPauseGameEvent ev)
    {
        if (!GameEvents.OnTogglingPause(ev).IsAllowed)
            return;
        GameEvents.OnToggledPause(new(ev.Paused) { Source = ev });
    }
}
