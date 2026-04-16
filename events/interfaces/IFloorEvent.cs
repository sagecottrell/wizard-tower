using wizardtower.state;

namespace wizardtower.events.interfaces;

public interface IFloorEvent
{
    FloorState Floor { get; set; }
}
