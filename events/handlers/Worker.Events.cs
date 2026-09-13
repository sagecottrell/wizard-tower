
using System.Diagnostics.CodeAnalysis;
using wizardtower.events.features;
using wizardtower.events.Worker;

namespace wizardtower.events.handlers;
    
public static partial class WorkerEvents {    
    public static Event<WorkerDispatchedEvent> Dispatched { get; set; } = new();
    public static WorkerDispatchedEvent OnDispatched(WorkerDispatchedEvent e) => Dispatched.InvokeSafely(e);
    public static WorkerDispatchedEvent OnDispatched(WorkerDispatchedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Dispatched.InvokeSafely(e); 
    }    
    public static Event<WorkerDispatchingEvent> Dispatching { get; set; } = new();
    public static WorkerDispatchingEvent OnDispatching(WorkerDispatchingEvent e) => Dispatching.InvokeSafely(e);
    public static WorkerDispatchingEvent OnDispatching(WorkerDispatchingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Dispatching.InvokeSafely(e); 
    }    
    public static Event<WorkerEnteredTransportEvent> EnteredTransport { get; set; } = new();
    public static WorkerEnteredTransportEvent OnEnteredTransport(WorkerEnteredTransportEvent e) => EnteredTransport.InvokeSafely(e);
    public static WorkerEnteredTransportEvent OnEnteredTransport(WorkerEnteredTransportEvent e, BaseEvent source) { 
        e.Source = source; 
        return EnteredTransport.InvokeSafely(e); 
    }    
    public static Event<WorkerEnteringTransportEvent> EnteringTransport { get; set; } = new();
    public static WorkerEnteringTransportEvent OnEnteringTransport(WorkerEnteringTransportEvent e) => EnteringTransport.InvokeSafely(e);
    public static WorkerEnteringTransportEvent OnEnteringTransport(WorkerEnteringTransportEvent e, BaseEvent source) { 
        e.Source = source; 
        return EnteringTransport.InvokeSafely(e); 
    }    
    public static Event<WorkerReachedCheckpointEvent> ReachedCheckpoint { get; set; } = new();
    public static WorkerReachedCheckpointEvent OnReachedCheckpoint(WorkerReachedCheckpointEvent e) => ReachedCheckpoint.InvokeSafely(e);
    public static WorkerReachedCheckpointEvent OnReachedCheckpoint(WorkerReachedCheckpointEvent e, BaseEvent source) { 
        e.Source = source; 
        return ReachedCheckpoint.InvokeSafely(e); 
    }    
    public static Event<WorkerReachingCheckpointEvent> ReachingCheckpoint { get; set; } = new();
    public static WorkerReachingCheckpointEvent OnReachingCheckpoint(WorkerReachingCheckpointEvent e) => ReachingCheckpoint.InvokeSafely(e);
    public static WorkerReachingCheckpointEvent OnReachingCheckpoint(WorkerReachingCheckpointEvent e, BaseEvent source) { 
        e.Source = source; 
        return ReachingCheckpoint.InvokeSafely(e); 
    }    
    public static bool TryDispatching(WorkerDispatchingEvent pre, [NotNullWhen(true)] out WorkerDispatchedEvent? e) {
        e = null;
        if (OnDispatching(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryEnteringTransport(WorkerEnteringTransportEvent pre, [NotNullWhen(true)] out WorkerEnteredTransportEvent? e) {
        e = null;
        if (OnEnteringTransport(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryReachingCheckpoint(WorkerReachingCheckpointEvent pre, [NotNullWhen(true)] out WorkerReachedCheckpointEvent? e) {
        e = null;
        if (OnReachingCheckpoint(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }
}