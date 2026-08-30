
using System.Diagnostics.CodeAnalysis;
using wizardtower.events.features;
using wizardtower.events.Game;

namespace wizardtower.events.handlers;
    
public static partial class GameEvents {    
    public static Event<GamePauseToggledEvent> PauseToggled { get; set; } = new();
    public static GamePauseToggledEvent OnPauseToggled(GamePauseToggledEvent e) => PauseToggled.InvokeSafely(e);
    public static GamePauseToggledEvent OnPauseToggled(GamePauseToggledEvent e, BaseEvent source) { 
        e.Source = source; 
        return PauseToggled.InvokeSafely(e); 
    }    
    public static Event<GamePauseTogglingEvent> PauseToggling { get; set; } = new();
    public static GamePauseTogglingEvent OnPauseToggling(GamePauseTogglingEvent e) => PauseToggling.InvokeSafely(e);
    public static GamePauseTogglingEvent OnPauseToggling(GamePauseTogglingEvent e, BaseEvent source) { 
        e.Source = source; 
        return PauseToggling.InvokeSafely(e); 
    }    
    public static bool TryPauseToggling(GamePauseTogglingEvent pre, [NotNullWhen(true)] out GamePauseToggledEvent? e) {
        e = null;
        if (OnPauseToggling(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }
}