/**
Generated from ./events/Room/RoomProcessingIncreasedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;
using wizardtower.state.room_functions;

namespace wizardtower.events.Room;

public class RoomProcessingIncreasingEvent(RoomState roomState, RoomConvertResourcesState state) : BaseEvent, IDeniableEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public RoomState RoomState { get; set; } = roomState;
    public RoomConvertResourcesState State { get; set; } = state;
    public double AmountIncreased { get; set; }
}


public static class RoomProcessingIncreasedEventExtensions {
    public static RoomProcessingIncreasedEvent Into(this RoomProcessingIncreasingEvent old) {
        return new(roomState: old.RoomState, state: old.State) { Source = old, Input = old.Input, };
    }
}