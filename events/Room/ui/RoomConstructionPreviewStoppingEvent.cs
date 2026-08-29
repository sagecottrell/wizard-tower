/**
Generated from ./events/Room/ui/RoomConstructionPreviewStoppedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

public partial class RoomConstructionPreviewStoppingEvent(TowerState towerState) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
}


public static class RoomConstructionPreviewStoppedEventExtensions {
    public static RoomConstructionPreviewStoppedEvent Into(this RoomConstructionPreviewStoppingEvent old) {
        return new(towerState: old.TowerState) { Source = old, Input = old.Input, };
    }
}