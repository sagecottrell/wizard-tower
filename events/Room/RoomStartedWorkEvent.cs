using wizardtower.events.interfaces;
using wizardtower.resource_types.room_functions;
using wizardtower.state;
using wizardtower.state.room_functions;

namespace wizardtower.events.Room;

public class RoomStartedWorkEvent(TowerState towerState, RoomState roomState, RoomConvertResourcesDefinition resourcesConsumed, RoomConvertResourcesState roomConvertResourcesState) : BaseEvent, ITowerEvent, IRoomEvent
{
    public TowerState TowerState { get; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public RoomConvertResourcesDefinition ResourcesConsumed { get; set; } = resourcesConsumed;
    public RoomConvertResourcesState RoomConvertResourcesState { get; set; } = roomConvertResourcesState;
}
