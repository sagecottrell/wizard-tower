using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomReceivedResourcesEvent(TowerState towerState, RoomState roomState, NumericDict<ItemDefinition, uint> resources) : BaseEvent, ITowerEvent, IRoomEvent
{
    public TowerState TowerState { get; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public NumericDict<ItemDefinition, uint> Resources { get; set; } = resources;
}
