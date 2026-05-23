using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Worker;

public class WorkerDispatchedEvent(TowerState towerState, WorkerState workerState) : BaseEvent, ITowerEvent, IWorkerEvent
{
    public TowerState TowerState { get; } = towerState;
    public WorkerState WorkerState { get; set; } = workerState;
}
