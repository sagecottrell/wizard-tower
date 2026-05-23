using wizardtower.events.features;
using wizardtower.events.Worker;

namespace wizardtower.events.handlers;

public static class WorkerEvents
{
    public static Event<WorkerDispatchingEvent> Dispatching { get; set; } = new();
    public static Event<WorkerDispatchedEvent> Dispatched { get; set; } = new();

    public static Event<WorkerEnteringTransportEvent> EnteringTransport { get; set; } = new();
    public static Event<WorkerEnteredTransportEvent> EnteredTransport { get; set; } = new(); 

    public static WorkerDispatchingEvent OnrDispatching(WorkerDispatchingEvent e) => Dispatching.InvokeSafely(e);
    public static WorkerDispatchedEvent OnDispatched(WorkerDispatchedEvent e) => Dispatched.InvokeSafely(e);

    public static WorkerEnteringTransportEvent OnEnteringTransport(WorkerEnteringTransportEvent e) => EnteringTransport.InvokeSafely(e);
    public static WorkerEnteredTransportEvent OnEnteredTransport(WorkerEnteredTransportEvent e) => EnteredTransport.InvokeSafely(e);
}
