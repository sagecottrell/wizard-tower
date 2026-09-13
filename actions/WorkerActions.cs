using wizardtower.events.handlers;
using wizardtower.events.Worker;

namespace wizardtower.actions;

public static class WorkerActions
{
    public static void Dispatch(WorkerDispatchingEvent @event)
    {
        if (!WorkerEvents.TryDispatching(@event, out var e))
            return;
        @event.TowerState.SpawnWorker(e.WorkerState);
        WorkerEvents.OnDispatched(e);
    }
}
