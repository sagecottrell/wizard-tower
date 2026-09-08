/**
Generated from ./events/Room/ui/RoomDeliveryPlanningStartedEvent.cs
**/

using System.Collections.Generic;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

/// <summary>
/// <para>
/// When the user begins the delivery planning process via the room details UI
/// </para>
/// </summary>
public class RoomDeliveryPlanningStartingEvent(TowerState towerState, RoomState roomState, List<ItemDefinition> itemDefinitions) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;

    public RoomState RoomState { get; set; } = roomState;

    public List<ItemDefinition> ItemDefinitions { get; set; } = itemDefinitions;
}


public static class RoomDeliveryPlanningStartedEventExtensions {
    public static RoomDeliveryPlanningStartedEvent Into(this RoomDeliveryPlanningStartingEvent old) {
        return new(towerState: old.TowerState, roomState: old.RoomState, itemDefinitions: old.ItemDefinitions) { Source = old, };
    }

    public static RoomDeliveryPlanningStartingEvent RoomDeliveryPlanningStartingEvent(this IEvent ev, TowerState towerState, RoomState roomState, List<ItemDefinition> itemDefinitions)
    {
        return new(towerState, roomState, itemDefinitions) { Source = ev };
    }
}