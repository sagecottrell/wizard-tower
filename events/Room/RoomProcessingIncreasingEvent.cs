using wizardtower.events.interfaces;
using wizardtower.state;
using wizardtower.state.room_functions;

namespace wizardtower.events.Room;

public class RoomProcessingIncreasingEvent(RoomState roomState, RoomConvertResourcesState state) : BaseEvent, IDeniableEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public RoomState RoomState { get; } = roomState;
    public RoomConvertResourcesState State { get; set; } = state;
    public double AmountIncreased { get; set; }
}
