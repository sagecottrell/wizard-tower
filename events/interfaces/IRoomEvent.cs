
using wizardtower.state;

namespace wizardtower.events.interfaces;

public interface IRoomEvent
{
    RoomState RoomState { get; set; }
}
