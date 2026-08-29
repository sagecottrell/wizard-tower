/**
Generated from ./events/Room/ui/RoomConstructionStoppedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

public partial class RoomConstructionStoppingEvent(TowerState tower, RoomDefinition roomDefinition) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
}


public static class RoomConstructionStoppedEventExtensions {
    public static RoomConstructionStoppedEvent Into(this RoomConstructionStoppingEvent old) {
        return new(tower: old.TowerState, roomDefinition: old.RoomDefinition) { Source = old, Input = old.Input, };
    }
}