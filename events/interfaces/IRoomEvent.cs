
using wizardtower.state;

namespace wizardtower.events.interfaces;

public interface IRoomEvent
{
    RoomState Room { get; set; }
}
