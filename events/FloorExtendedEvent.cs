using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events;

public partial class FloorExtendedEvent(TowerState towerState, FloorState floor, uint extendedLeft, uint extendedRight) : BaseEvent, IDebug, ITowerEvent, IEvent
{
    public TowerState TowerState { get; } = towerState;
    public FloorState Floor { get; } = floor;
    public uint ExtendedLeft { get; } = extendedLeft;
    public uint ExtendedRight { get; } = extendedRight;
    public uint ExtensionAmount => ExtendedLeft + ExtendedRight;
}
