/**
Generated from ./events/Worker/WorkerReachedCheckpointEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Worker;

/// <summary>
/// When a worker reaches a checkpoint (a destination room or transport) along their delivery path
/// </summary>
public class WorkerReachingCheckpointEvent(TowerState towerState, WorkerState worker) : BaseEvent, IDeniableEvent, ITowerEvent, IWorkerEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public WorkerState WorkerState { get; set; } = worker;
}


public static class WorkerReachedCheckpointEventExtensions {
    public static WorkerReachedCheckpointEvent Into(this WorkerReachingCheckpointEvent old) {
        return new(towerState: old.TowerState, worker: old.WorkerState) { Source = old, };
    }

    public static WorkerReachingCheckpointEvent WorkerReachingCheckpointEvent(this IEvent ev, TowerState towerState, WorkerState worker)
    {
        return new(towerState, worker) { Source = ev };
    }
}