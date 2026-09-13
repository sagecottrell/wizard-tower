using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Worker;

/// <summary>
/// When a worker is dispatched with the specified state
/// </summary>
public class WorkerDispatchedEvent(TowerState towerState, RoomState roomState, RoomState targetRoom, ItemDefinition item, uint amount, WorkerDefinition def) : BaseEvent, ITowerEvent
{
    public TowerState TowerState { get; } = towerState;
    public RoomState RoomState { get; } = roomState;
    public RoomState TargetRoom { get; } = targetRoom;
    public ItemDefinition Item { get; } = item;
    public uint Amount { get; } = amount;
    public WorkerDefinition WorkerDefinition { get; } = def;

    public WorkerState WorkerState { get; } = new(def)
    {
        DestinationRoomId = targetRoom.Id,
        FloorPosition = roomState.FloorPosition,
        Elevation = roomState.Elevation,
        PayloadAmount = amount,
        PayloadKind = item,
        SourceRoomId = roomState.Id,
        WorkerDefinition = def,
    };
}
