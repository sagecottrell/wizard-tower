using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Floor.ui;

public partial class FloorExtensionShowedEvent(TowerState towerState, FloorDefinition floorDefinition, int elevation, int left, int right) : BaseEvent, IDebug, ITowerEvent, IFloorDefinitionEvent
{
    public TowerState TowerState { get; } = towerState;
    public FloorDefinition FloorDefinition { get; set; } = floorDefinition;
    public int Elevation { get; set; } = elevation;
    public int Left { get; set; } = left;
    public int Right { get; set; } = right;
}
