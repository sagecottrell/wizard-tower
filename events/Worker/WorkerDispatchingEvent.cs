/**
Generated from ./events/Worker/WorkerDispatchedEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Worker;

public class WorkerDispatchingEvent(TowerState towerState, WorkerState workerState) : BaseEvent, IDeniableEvent, ITowerEvent, IWorkerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public WorkerState WorkerState { get; set; } = workerState;
}


public static class WorkerDispatchedEventExtensions {
    public static WorkerDispatchedEvent Into(this WorkerDispatchingEvent old) {
        return new(towerState: old.TowerState, workerState: old.WorkerState) { Source = old, };
    }

    public static WorkerDispatchingEvent WorkerDispatchingEvent(this IEvent ev, TowerState towerState, WorkerState workerState)
    {
        return new(towerState, workerState) { Source = ev };
    }
}