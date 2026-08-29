using wizardtower.events.Game;
using wizardtower.events.handlers;

namespace wizardtower.actions;

public static class GameActions
{
	public static void SetPause(GamePauseTogglingEvent ev)
	{
		if (!GameEvents.OnPauseToggling(ev).IsAllowed)
			return;
		GameEvents.OnPauseToggled(ev.Into());
	}
}
