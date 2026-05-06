using wizardtower.events.features;

namespace wizardtower.events.handlers;

public static class TowerEvents
{
    public static Event<TowerResourceChangingEvent> TowerResourceChanging { get; set; } = new();
    public static Event<TowerResourceChangedEvent> TowerResourceChanged { get; set; } = new();

    public static TowerResourceChangingEvent OnTowerResourceChanging(TowerResourceChangingEvent e) => TowerResourceChanging.InvokeSafely(e);
    public static TowerResourceChangedEvent OnTowerResourceChanged(TowerResourceChangedEvent e) => TowerResourceChanged.InvokeSafely(e);
}
