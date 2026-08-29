namespace wizardtower.events.Game;

public class GamePauseToggledEvent(bool paused) : BaseEvent
{
    public bool Paused { get; } = paused;
}
