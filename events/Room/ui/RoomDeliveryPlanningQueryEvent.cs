using System.Collections.Generic;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room.ui;


/// <summary>
/// <para>Used to get a list of next possible destinations for the delivery path.</para>
/// <para>the intention is that rooms and transports will listen for this event, look at <see cref="Path"/>
/// and add items to <see cref="NextValidPositions"/>.</para>
/// </summary>
public class RoomDeliveryPlanningQueryEvent(TowerState towerState, RoomState startingRoom, ItemDefinition itemDefinition, List<RoomStateWorkerPath> path) : BaseEvent, ITowerEvent
{
    public TowerState TowerState { get; } = towerState;

    public RoomState StartingRoom { get; } = startingRoom;

    public ItemDefinition ItemDefinition { get; } = itemDefinition;

    public List<RoomStateWorkerPath> Path { get; } = path;

    public HashSet<(int elevation, int position)> NextValidPositions { get; } = [];
}
