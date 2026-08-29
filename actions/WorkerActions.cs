using wizardtower.events.handlers;
using wizardtower.events.Worker;

namespace wizardtower.actions;

public static class WorkerActions
{
    public static void Dispatch(WorkerDispatchingEvent @event)
    {
        if (!WorkerEvents.OnDispatching(@event).IsAllowed)
            return;
        if (@event.WorkerState.WalkingAbout)
            return;
        @event.TowerState.SpawnWorker(@event.WorkerState);
        WorkerEvents.OnDispatched(@event.Into());
    }
}
