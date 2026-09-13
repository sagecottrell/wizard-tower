/**
Generated from ./events/Room/RoomResourcesDiminishedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room;

/// <summary>
/// When a room loses resources for any reason. inspect the source event for more information
/// </summary>
public class RoomResourcesDiminishingEvent(TowerState towerState, RoomState roomState, NumericDict<ItemDefinition, uint> amount) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public NumericDict<ItemDefinition, uint> Amount { get; set; } = amount;
}


public static class RoomResourcesDiminishedEventExtensions {
    public static RoomResourcesDiminishedEvent Into(this RoomResourcesDiminishingEvent old) {
        return new(towerState: old.TowerState, roomState: old.RoomState, amount: old.Amount) { Source = old, };
    }

    public static RoomResourcesDiminishingEvent RoomResourcesDiminishingEvent(this IEvent ev, TowerState towerState, RoomState roomState, NumericDict<ItemDefinition, uint> amount)
    {
        return new(towerState, roomState, amount) { Source = ev };
    }
}