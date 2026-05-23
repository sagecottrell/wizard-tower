using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Worker;

public class WorkerDispatchingEvent(TowerState towerState, WorkerState workerState) : BaseEvent, IDeniableEvent, ITowerEvent, IWorkerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public WorkerState WorkerState { get; set; } = workerState;
}
