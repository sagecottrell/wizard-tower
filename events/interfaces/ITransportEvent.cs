using wizardtower.state;

namespace wizardtower.events.interfaces;

public interface ITransportEvent
{
    TransportState Transport { get; set; }
}
