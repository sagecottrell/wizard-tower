
using wizardtower.events.features;
using wizardtower.events.Tower;

namespace wizardtower.events.handlers;
    
public static partial class TowerEvents {
    public static Event<TowerResourceChangedEvent> ResourceChanged { get; set; } = new();
    public static TowerResourceChangedEvent OnResourceChanged(TowerResourceChangedEvent e) => ResourceChanged.InvokeSafely(e);
    public static Event<TowerResourceChangingEvent> ResourceChanging { get; set; } = new();
    public static TowerResourceChangingEvent OnResourceChanging(TowerResourceChangingEvent e) => ResourceChanging.InvokeSafely(e);
}