using wizardtower.state;

namespace wizardtower.events.interfaces;

public interface ITransportEvent
{
    TransportState TransportState { get; set; }
}
