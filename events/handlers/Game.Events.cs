
using wizardtower.events.features;
using wizardtower.events.Game;

namespace wizardtower.events.handlers;
    
public static partial class GameEvents {
    public static Event<GamePauseToggledEvent> PauseToggled { get; set; } = new();
    public static GamePauseToggledEvent OnPauseToggled(GamePauseToggledEvent e) => PauseToggled.InvokeSafely(e);
    public static Event<GamePauseTogglingEvent> PauseToggling { get; set; } = new();
    public static GamePauseTogglingEvent OnPauseToggling(GamePauseTogglingEvent e) => PauseToggling.InvokeSafely(e);
}