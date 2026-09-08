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
public class RoomDeliveryPlanningStartedEvent(TowerState towerState, RoomState roomState, List<ItemDefinition> itemDefinitions) : BaseEvent, ITowerEvent, IRoomEvent
{
    public TowerState TowerState { get; } = towerState;

    public RoomState RoomState { get; } = roomState;

    public List<ItemDefinition> ItemDefinitions { get; } = itemDefinitions;
}
