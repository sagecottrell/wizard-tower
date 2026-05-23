using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomReceivingResourcesEvent(TowerState towerState, RoomState roomState, NumericDict<ItemDefinition, uint> resources) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public NumericDict<ItemDefinition, uint> Resources { get; set; } = resources;
}
