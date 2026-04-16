using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class RoomConstructionStoppingEvent(TowerState towerState, RoomDefinition roomDefinition) : BaseEvent, IAllowableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
}
