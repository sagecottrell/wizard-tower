using wizardtower.events.interfaces;

namespace wizardtower.events.Game;

public class TogglingPauseGameEvent(bool paused) : BaseEvent, IDeniableEvent
{
    public bool IsAllowed { get; set; }
    public bool Paused { get; set; } = paused;
}
