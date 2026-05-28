using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomProducedResourcesEvent(TowerState towerState, RoomState roomState) : BaseEvent, ITowerEvent, IRoomEvent
{
    public TowerState TowerState { get; set; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public NumericDict<ItemDefinition, uint>? Output { get; set; }
    public bool ResetProductionProgress { get; set; } = true;
}
