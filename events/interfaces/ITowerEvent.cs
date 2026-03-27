using wizardtower.state;

namespace wizardtower.events.interfaces;

public interface ITowerEvent
{
    TowerState TowerState { get; }
}
