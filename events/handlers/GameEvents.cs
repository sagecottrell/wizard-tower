using wizardtower.events.features;
using wizardtower.events.Game;

namespace wizardtower.events.handlers;

public static class GameEvents
{
    public static Event<TogglingPauseGameEvent> TogglingPause { get; set; } = new();
    public static Event<ToggledPauseGameEvent> ToggledPause { get; set; } = new();

    public static TogglingPauseGameEvent OnTogglingPause(TogglingPauseGameEvent e) => TogglingPause.InvokeSafely(e);
    public static ToggledPauseGameEvent OnToggledPause(ToggledPauseGameEvent e) => ToggledPause.InvokeSafely(e);

}
