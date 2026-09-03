/**
Generated from ./events/Room/RoomConsumedResourcesEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomConsumingResourcesEvent(TowerState towerState, RoomState roomState, NumericDict<ItemDefinition, uint> amount) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public NumericDict<ItemDefinition, uint> Amount { get; set; } = amount;
}


public static class RoomConsumedResourcesEventExtensions {
    public static RoomConsumedResourcesEvent Into(this RoomConsumingResourcesEvent old) {
        return new(towerState: old.TowerState, roomState: old.RoomState, amount: old.Amount) { Source = old, };
    }

    public static RoomConsumingResourcesEvent RoomConsumingResourcesEvent(this IEvent ev, TowerState towerState, RoomState roomState, NumericDict<ItemDefinition, uint> amount)
    {
        return new(towerState, roomState, amount) { Source = ev };
    }
}