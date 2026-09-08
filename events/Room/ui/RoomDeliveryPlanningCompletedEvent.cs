using System.Collections.Generic;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

/// <summary>
/// <para>
/// When the user either completes or cancels the delivery planning process
/// </para>
/// </summary>
public class RoomDeliveryPlanningCompletedEvent(TowerState towerState, RoomState roomState, List<RoomStateWorkerPath> paths) : BaseEvent, ITowerEvent, IRoomEvent
{
    public TowerState TowerState { get; } = towerState;

    public RoomState RoomState { get; } = roomState;

    public List<RoomStateWorkerPath> Paths { get; } = paths;
}
