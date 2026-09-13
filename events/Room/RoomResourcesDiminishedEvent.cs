using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room;

/// <summary>
/// When a room loses resources for any reason. inspect the source event for more information
/// </summary>
public class RoomResourcesDiminishedEvent(TowerState towerState, RoomState roomState, NumericDict<ItemDefinition, uint> amount) : BaseEvent, ITowerEvent, IRoomEvent
{
    public TowerState TowerState { get; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public NumericDict<ItemDefinition, uint> Amount { get; } = amount;
}
