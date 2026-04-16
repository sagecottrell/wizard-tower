using wizardtower.resource_types;

namespace wizardtower.events.interfaces;

public interface IFloorDefinitionEvent
{
    FloorDefinition FloorDefinition { get; set; }
}
