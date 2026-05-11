using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Floor;

public partial class FloorConstructingEvent(TowerState tower, FloorState floor) : BaseEvent, IDebug, ITowerEvent, IDeniableEvent, IEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = tower;
    public FloorState Floor { get; } = floor;
}
