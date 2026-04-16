using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class RoomConstructionStoppedEvent(TowerState tower, RoomDefinition roomDefinition) : BaseEvent, IDebug, ITowerEvent
{
    public TowerState TowerState { get; } = tower;
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
}
