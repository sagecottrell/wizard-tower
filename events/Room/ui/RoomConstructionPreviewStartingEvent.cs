/**
Generated from ./events/Room/ui/RoomConstructionPreviewStartedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

public partial class RoomConstructionPreviewStartingEvent(TowerState towerState, RoomState previewState) : BaseEvent, IDeniableEvent, IDebug, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;

    public RoomState RoomState { get; set; } = previewState;
}


public static class RoomConstructionPreviewStartedEventExtensions {
    public static RoomConstructionPreviewStartedEvent Into(this RoomConstructionPreviewStartingEvent old) {
        return new(towerState: old.TowerState, previewState: old.RoomState) { Source = old, };
    }

    public static RoomConstructionPreviewStartingEvent RoomConstructionPreviewStartingEvent(this IEvent ev, TowerState towerState, RoomState previewState)
    {
        return new(towerState, previewState) { Source = ev };
    }
}