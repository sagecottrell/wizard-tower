namespace wizardtower.events.Game;

public class ToggledPauseGameEvent(bool paused) : BaseEvent
{
    public bool Paused { get; } = paused;
}
