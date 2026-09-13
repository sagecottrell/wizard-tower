using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Worker;

/// <summary>
/// When a worker reaches a checkpoint (a destination room or transport) along their delivery path
/// </summary>
public class WorkerReachedCheckpointEvent(TowerState towerState, WorkerState worker) : BaseEvent, ITowerEvent, IWorkerEvent
{
    public TowerState TowerState { get; } = towerState;
    public WorkerState WorkerState { get; set; } = worker;
}
