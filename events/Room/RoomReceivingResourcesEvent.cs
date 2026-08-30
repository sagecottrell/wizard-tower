/**
Generated from ./events/Room/RoomReceivedResourcesEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomReceivingResourcesEvent(TowerState towerState, RoomState roomState, NumericDict<ItemDefinition, uint> resources) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public NumericDict<ItemDefinition, uint> Resources { get; set; } = resources;
}


public static class RoomReceivedResourcesEventExtensions {
    public static RoomReceivedResourcesEvent Into(this RoomReceivingResourcesEvent old) {
        return new(towerState: old.TowerState, roomState: old.RoomState, resources: old.Resources) { Source = old, };
    }
}