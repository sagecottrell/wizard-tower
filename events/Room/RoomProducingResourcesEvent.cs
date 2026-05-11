using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.resource_types.room_functions;
using wizardtower.state;
using wizardtower.state.room_functions;

namespace wizardtower.events.Room;

public class RoomProducingResourcesEvent(TowerState towerState, RoomState roomState, RoomConvertResourcesDefinition roomConvertResourcesDefinition, RoomConvertResourcesState roomConvertResourcesState) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public RoomConvertResourcesDefinition RoomConvertResourcesDefinition { get; } = roomConvertResourcesDefinition;
    public RoomConvertResourcesState RoomConvertResourcesState { get; } = roomConvertResourcesState;
    public NumericDict<ItemDefinition, uint>? Output { get; set; }
    public bool ResetProductionProgress { get; set; } = true;
}
