
using wizardtower.events.features;
using wizardtower.events.Worker;

namespace wizardtower.events.handlers;
    
public static partial class WorkerEvents {
    public static Event<WorkerDispatchedEvent> Dispatched { get; set; } = new();
    public static WorkerDispatchedEvent OnDispatched(WorkerDispatchedEvent e) => Dispatched.InvokeSafely(e);
    public static Event<WorkerDispatchingEvent> Dispatching { get; set; } = new();
    public static WorkerDispatchingEvent OnDispatching(WorkerDispatchingEvent e) => Dispatching.InvokeSafely(e);
    public static Event<WorkerEnteredTransportEvent> EnteredTransport { get; set; } = new();
    public static WorkerEnteredTransportEvent OnEnteredTransport(WorkerEnteredTransportEvent e) => EnteredTransport.InvokeSafely(e);
    public static Event<WorkerEnteringTransportEvent> EnteringTransport { get; set; } = new();
    public static WorkerEnteringTransportEvent OnEnteringTransport(WorkerEnteringTransportEvent e) => EnteringTransport.InvokeSafely(e);
}