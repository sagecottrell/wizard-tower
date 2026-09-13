/**
Generated from ./events/Worker/WorkerDispatchedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Worker;

/// <summary>
/// When a worker is dispatched with the specified state
/// </summary>
public class WorkerDispatchingEvent(TowerState towerState, RoomState roomState, RoomState targetRoom, ItemDefinition item, uint amount, WorkerDefinition def) : BaseEvent, IDeniableEvent, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public RoomState TargetRoom { get; set; } = targetRoom;
    public ItemDefinition Item { get; set; } = item;
    public uint Amount { get; set; } = amount;
    public WorkerDefinition WorkerDefinition { get; set; } = def;

    public WorkerState WorkerState { get; set; } = new(def)
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


public static class WorkerDispatchedEventExtensions {
    public static WorkerDispatchedEvent Into(this WorkerDispatchingEvent old) {
        return new(towerState: old.TowerState, roomState: old.RoomState, targetRoom: old.TargetRoom, item: old.Item, amount: old.Amount, def: old.WorkerDefinition) { Source = old, };
    }

    public static WorkerDispatchingEvent WorkerDispatchingEvent(this IEvent ev, TowerState towerState, RoomState roomState, RoomState targetRoom, ItemDefinition item, uint amount, WorkerDefinition def)
    {
        return new(towerState, roomState, targetRoom, item, amount, def) { Source = ev };
    }
}