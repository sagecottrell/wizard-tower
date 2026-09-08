/**
Generated from ./events/Room/ui/RoomDeliveryPlanningCompletedEvent.cs
**/

using System.Collections.Generic;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

/// <summary>
/// <para>
/// When the user either completes or cancels the delivery planning process
/// </para>
/// </summary>
public class RoomDeliveryPlanningCompletingEvent(TowerState towerState, RoomState roomState, List<RoomStateWorkerPath> paths) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;

    public RoomState RoomState { get; set; } = roomState;

    public List<RoomStateWorkerPath> Paths { get; set; } = paths;
}


public static class RoomDeliveryPlanningCompletedEventExtensions {
    public static RoomDeliveryPlanningCompletedEvent Into(this RoomDeliveryPlanningCompletingEvent old) {
        return new(towerState: old.TowerState, roomState: old.RoomState, paths: old.Paths) { Source = old, };
    }

    public static RoomDeliveryPlanningCompletingEvent RoomDeliveryPlanningCompletingEvent(this IEvent ev, TowerState towerState, RoomState roomState, List<RoomStateWorkerPath> paths)
    {
        return new(towerState, roomState, paths) { Source = ev };
    }
}