using wizardtower.events.interfaces;
using wizardtower.state;
using wizardtower.state.room_functions;

namespace wizardtower.events.Room;

public class RoomProcessingIncreasedEvent(RoomState roomState, RoomConvertResourcesState state) : BaseEvent, IRoomEvent
{
    public RoomState RoomState { get; } = roomState;
    public RoomConvertResourcesState State { get; set; } = state;
    public double AmountIncreased { get; set; }
}
