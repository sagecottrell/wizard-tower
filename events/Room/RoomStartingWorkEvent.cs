using wizardtower.events.interfaces;
using wizardtower.resource_types.room_functions;
using wizardtower.state;
using wizardtower.state.room_functions;

namespace wizardtower.events.Room;

public class RoomStartingWorkEvent(TowerState towerState, RoomState roomState, RoomConvertResourcesDefinition roomConvertResourcesDefinition, RoomConvertResourcesState roomConvertResourcesState) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public RoomConvertResourcesDefinition RoomConvertResourcesDefinition { get; set; } = roomConvertResourcesDefinition;
    public RoomConvertResourcesState RoomConvertResourcesState { get; set; } = roomConvertResourcesState;
}
