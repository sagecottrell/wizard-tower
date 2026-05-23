using wizardtower.events.handlers;
using wizardtower.events.Worker;

namespace wizardtower.actions;

public static class WorkerActions
{
    public static void Dispatch(WorkerDispatchingEvent @event)
    {
        if (!WorkerEvents.OnrDispatching(@event).IsAllowed)
            return;
        var worker = @event.WorkerState;
        if (@event.WorkerState.WalkingAbout)
            return;
        @event.TowerState.SpawnWorker(@event.WorkerState);
        WorkerEvents.OnDispatched(new(@event.TowerState, worker) { Source = @event });
    }
}
