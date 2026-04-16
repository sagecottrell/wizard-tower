using wizardtower.resource_types;

namespace wizardtower.events.interfaces;

public interface ITransportDefinitionEvent
{
    TransportDefinition TransportDefinition { get; set; }
}
