
using System.Diagnostics.CodeAnalysis;
using wizardtower.events.features;
using wizardtower.events.Room;

namespace wizardtower.events.handlers;
    
public static partial class RoomEvents {    
    public static Event<RoomProducedResourcesEvent> ProducedResources { get; set; } = new();
    public static RoomProducedResourcesEvent OnProducedResources(RoomProducedResourcesEvent e) => ProducedResources.InvokeSafely(e);
    public static RoomProducedResourcesEvent OnProducedResources(RoomProducedResourcesEvent e, BaseEvent source) { 
        e.Source = source; 
        return ProducedResources.InvokeSafely(e); 
    }    
    public static Event<RoomProducingResourcesEvent> ProducingResources { get; set; } = new();
    public static RoomProducingResourcesEvent OnProducingResources(RoomProducingResourcesEvent e) => ProducingResources.InvokeSafely(e);
    public static RoomProducingResourcesEvent OnProducingResources(RoomProducingResourcesEvent e, BaseEvent source) { 
        e.Source = source; 
        return ProducingResources.InvokeSafely(e); 
    }    
    public static Event<RoomDestroyedEvent> Destroyed { get; set; } = new();
    public static RoomDestroyedEvent OnDestroyed(RoomDestroyedEvent e) => Destroyed.InvokeSafely(e);
    public static RoomDestroyedEvent OnDestroyed(RoomDestroyedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Destroyed.InvokeSafely(e); 
    }    
    public static Event<RoomDestroyingEvent> Destroying { get; set; } = new();
    public static RoomDestroyingEvent OnDestroying(RoomDestroyingEvent e) => Destroying.InvokeSafely(e);
    public static RoomDestroyingEvent OnDestroying(RoomDestroyingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Destroying.InvokeSafely(e); 
    }    
    public static Event<RoomStartedWorkEvent> StartedWork { get; set; } = new();
    public static RoomStartedWorkEvent OnStartedWork(RoomStartedWorkEvent e) => StartedWork.InvokeSafely(e);
    public static RoomStartedWorkEvent OnStartedWork(RoomStartedWorkEvent e, BaseEvent source) { 
        e.Source = source; 
        return StartedWork.InvokeSafely(e); 
    }    
    public static Event<RoomStartingWorkEvent> StartingWork { get; set; } = new();
    public static RoomStartingWorkEvent OnStartingWork(RoomStartingWorkEvent e) => StartingWork.InvokeSafely(e);
    public static RoomStartingWorkEvent OnStartingWork(RoomStartingWorkEvent e, BaseEvent source) { 
        e.Source = source; 
        return StartingWork.InvokeSafely(e); 
    }    
    public static Event<RoomAssignedOutputEvent> AssignedOutput { get; set; } = new();
    public static RoomAssignedOutputEvent OnAssignedOutput(RoomAssignedOutputEvent e) => AssignedOutput.InvokeSafely(e);
    public static RoomAssignedOutputEvent OnAssignedOutput(RoomAssignedOutputEvent e, BaseEvent source) { 
        e.Source = source; 
        return AssignedOutput.InvokeSafely(e); 
    }    
    public static Event<RoomAssigningOutputEvent> AssigningOutput { get; set; } = new();
    public static RoomAssigningOutputEvent OnAssigningOutput(RoomAssigningOutputEvent e) => AssigningOutput.InvokeSafely(e);
    public static RoomAssigningOutputEvent OnAssigningOutput(RoomAssigningOutputEvent e, BaseEvent source) { 
        e.Source = source; 
        return AssigningOutput.InvokeSafely(e); 
    }    
    public static Event<RoomConstructedEvent> Constructed { get; set; } = new();
    public static RoomConstructedEvent OnConstructed(RoomConstructedEvent e) => Constructed.InvokeSafely(e);
    public static RoomConstructedEvent OnConstructed(RoomConstructedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Constructed.InvokeSafely(e); 
    }    
    public static Event<RoomConstructingEvent> Constructing { get; set; } = new();
    public static RoomConstructingEvent OnConstructing(RoomConstructingEvent e) => Constructing.InvokeSafely(e);
    public static RoomConstructingEvent OnConstructing(RoomConstructingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Constructing.InvokeSafely(e); 
    }    
    public static Event<RoomProcessingIncreasedEvent> ProcessingIncreased { get; set; } = new();
    public static RoomProcessingIncreasedEvent OnProcessingIncreased(RoomProcessingIncreasedEvent e) => ProcessingIncreased.InvokeSafely(e);
    public static RoomProcessingIncreasedEvent OnProcessingIncreased(RoomProcessingIncreasedEvent e, BaseEvent source) { 
        e.Source = source; 
        return ProcessingIncreased.InvokeSafely(e); 
    }    
    public static Event<RoomProcessingIncreasingEvent> ProcessingIncreasing { get; set; } = new();
    public static RoomProcessingIncreasingEvent OnProcessingIncreasing(RoomProcessingIncreasingEvent e) => ProcessingIncreasing.InvokeSafely(e);
    public static RoomProcessingIncreasingEvent OnProcessingIncreasing(RoomProcessingIncreasingEvent e, BaseEvent source) { 
        e.Source = source; 
        return ProcessingIncreasing.InvokeSafely(e); 
    }    
    public static Event<RoomResourcesDiminishedEvent> ResourcesDiminished { get; set; } = new();
    public static RoomResourcesDiminishedEvent OnResourcesDiminished(RoomResourcesDiminishedEvent e) => ResourcesDiminished.InvokeSafely(e);
    public static RoomResourcesDiminishedEvent OnResourcesDiminished(RoomResourcesDiminishedEvent e, BaseEvent source) { 
        e.Source = source; 
        return ResourcesDiminished.InvokeSafely(e); 
    }    
    public static Event<RoomResourcesDiminishingEvent> ResourcesDiminishing { get; set; } = new();
    public static RoomResourcesDiminishingEvent OnResourcesDiminishing(RoomResourcesDiminishingEvent e) => ResourcesDiminishing.InvokeSafely(e);
    public static RoomResourcesDiminishingEvent OnResourcesDiminishing(RoomResourcesDiminishingEvent e, BaseEvent source) { 
        e.Source = source; 
        return ResourcesDiminishing.InvokeSafely(e); 
    }    
    public static Event<RoomConsumedResourcesEvent> ConsumedResources { get; set; } = new();
    public static RoomConsumedResourcesEvent OnConsumedResources(RoomConsumedResourcesEvent e) => ConsumedResources.InvokeSafely(e);
    public static RoomConsumedResourcesEvent OnConsumedResources(RoomConsumedResourcesEvent e, BaseEvent source) { 
        e.Source = source; 
        return ConsumedResources.InvokeSafely(e); 
    }    
    public static Event<RoomConsumingResourcesEvent> ConsumingResources { get; set; } = new();
    public static RoomConsumingResourcesEvent OnConsumingResources(RoomConsumingResourcesEvent e) => ConsumingResources.InvokeSafely(e);
    public static RoomConsumingResourcesEvent OnConsumingResources(RoomConsumingResourcesEvent e, BaseEvent source) { 
        e.Source = source; 
        return ConsumingResources.InvokeSafely(e); 
    }    
    public static Event<RoomReceivedResourcesEvent> ReceivedResources { get; set; } = new();
    public static RoomReceivedResourcesEvent OnReceivedResources(RoomReceivedResourcesEvent e) => ReceivedResources.InvokeSafely(e);
    public static RoomReceivedResourcesEvent OnReceivedResources(RoomReceivedResourcesEvent e, BaseEvent source) { 
        e.Source = source; 
        return ReceivedResources.InvokeSafely(e); 
    }    
    public static Event<RoomReceivingResourcesEvent> ReceivingResources { get; set; } = new();
    public static RoomReceivingResourcesEvent OnReceivingResources(RoomReceivingResourcesEvent e) => ReceivingResources.InvokeSafely(e);
    public static RoomReceivingResourcesEvent OnReceivingResources(RoomReceivingResourcesEvent e, BaseEvent source) { 
        e.Source = source; 
        return ReceivingResources.InvokeSafely(e); 
    }    
    public static Event<RoomStoppedWorkEvent> StoppedWork { get; set; } = new();
    public static RoomStoppedWorkEvent OnStoppedWork(RoomStoppedWorkEvent e) => StoppedWork.InvokeSafely(e);
    public static RoomStoppedWorkEvent OnStoppedWork(RoomStoppedWorkEvent e, BaseEvent source) { 
        e.Source = source; 
        return StoppedWork.InvokeSafely(e); 
    }    
    public static Event<RoomStoppingWorkEvent> StoppingWork { get; set; } = new();
    public static RoomStoppingWorkEvent OnStoppingWork(RoomStoppingWorkEvent e) => StoppingWork.InvokeSafely(e);
    public static RoomStoppingWorkEvent OnStoppingWork(RoomStoppingWorkEvent e, BaseEvent source) { 
        e.Source = source; 
        return StoppingWork.InvokeSafely(e); 
    }    
    public static bool TryProducingResources(RoomProducingResourcesEvent pre, [NotNullWhen(true)] out RoomProducedResourcesEvent? e) {
        e = null;
        if (OnProducingResources(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryDestroying(RoomDestroyingEvent pre, [NotNullWhen(true)] out RoomDestroyedEvent? e) {
        e = null;
        if (OnDestroying(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryStartingWork(RoomStartingWorkEvent pre, [NotNullWhen(true)] out RoomStartedWorkEvent? e) {
        e = null;
        if (OnStartingWork(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryAssigningOutput(RoomAssigningOutputEvent pre, [NotNullWhen(true)] out RoomAssignedOutputEvent? e) {
        e = null;
        if (OnAssigningOutput(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryConstructing(RoomConstructingEvent pre, [NotNullWhen(true)] out RoomConstructedEvent? e) {
        e = null;
        if (OnConstructing(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryProcessingIncreasing(RoomProcessingIncreasingEvent pre, [NotNullWhen(true)] out RoomProcessingIncreasedEvent? e) {
        e = null;
        if (OnProcessingIncreasing(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryResourcesDiminishing(RoomResourcesDiminishingEvent pre, [NotNullWhen(true)] out RoomResourcesDiminishedEvent? e) {
        e = null;
        if (OnResourcesDiminishing(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryConsumingResources(RoomConsumingResourcesEvent pre, [NotNullWhen(true)] out RoomConsumedResourcesEvent? e) {
        e = null;
        if (OnConsumingResources(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryReceivingResources(RoomReceivingResourcesEvent pre, [NotNullWhen(true)] out RoomReceivedResourcesEvent? e) {
        e = null;
        if (OnReceivingResources(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }    
    public static bool TryStoppingWork(RoomStoppingWorkEvent pre, [NotNullWhen(true)] out RoomStoppedWorkEvent? e) {
        e = null;
        if (OnStoppingWork(pre).IsAllowed) {
            e = pre.Into();
            return true;
        }
        return false;
    }
}