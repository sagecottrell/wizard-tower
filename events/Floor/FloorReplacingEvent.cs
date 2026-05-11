using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Floor;

public partial class FloorReplacingEvent(TowerState towerState, FloorState floor, FloorDefinition newDefinition) : BaseEvent, IDebug, ITowerEvent, IDeniableEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public FloorState Floor { get; } = floor;
    public FloorDefinition NewDefinition { get; } = newDefinition;
}
