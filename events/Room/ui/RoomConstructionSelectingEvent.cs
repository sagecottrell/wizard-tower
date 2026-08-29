/**
Generated from ./events/Room/ui/RoomConstructionSelectedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

public partial class RoomConstructionSelectingEvent(TowerState tower, RoomDefinition roomDefinition) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = tower;
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
}


public static class RoomConstructionSelectedEventExtensions {
    public static RoomConstructionSelectedEvent Into(this RoomConstructionSelectingEvent old) {
        return new(tower: old.TowerState, roomDefinition: old.RoomDefinition) { Source = old, Input = old.Input, };
    }
}