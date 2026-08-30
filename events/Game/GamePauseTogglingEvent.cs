/**
Generated from ./events/Game/GamePauseToggledEvent.cs
**/

namespace wizardtower.events.Game;

public class GamePauseTogglingEvent(bool paused) : BaseEvent
{
    public bool IsAllowed { get; set; } = true;
    public bool Paused { get; set; } = paused;
}


public static class GamePauseToggledEventExtensions {
    public static GamePauseToggledEvent Into(this GamePauseTogglingEvent old) {
        return new(paused: old.Paused) { Source = old, };
    }
}