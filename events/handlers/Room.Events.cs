
using wizardtower.events.features;
using wizardtower.events.Room;

namespace wizardtower.events.handlers;
    
public static partial class RoomEvents {
    public static Event<RoomProducedResourcesEvent> ProducedResources { get; set; } = new();
    public static RoomProducedResourcesEvent OnProducedResources(RoomProducedResourcesEvent e) => ProducedResources.InvokeSafely(e);
    public static Event<RoomProducingResourcesEvent> ProducingResources { get; set; } = new();
    public static RoomProducingResourcesEvent OnProducingResources(RoomProducingResourcesEvent e) => ProducingResources.InvokeSafely(e);
    public static Event<RoomDestroyedEvent> Destroyed { get; set; } = new();
    public static RoomDestroyedEvent OnDestroyed(RoomDestroyedEvent e) => Destroyed.InvokeSafely(e);
    public static Event<RoomDestroyingEvent> Destroying { get; set; } = new();
    public static RoomDestroyingEvent OnDestroying(RoomDestroyingEvent e) => Destroying.InvokeSafely(e);
    public static Event<RoomStartedWorkEvent> StartedWork { get; set; } = new();
    public static RoomStartedWorkEvent OnStartedWork(RoomStartedWorkEvent e) => StartedWork.InvokeSafely(e);
    public static Event<RoomStartingWorkEvent> StartingWork { get; set; } = new();
    public static RoomStartingWorkEvent OnStartingWork(RoomStartingWorkEvent e) => StartingWork.InvokeSafely(e);
    public static Event<RoomAssignedOutputEvent> AssignedOutput { get; set; } = new();
    public static RoomAssignedOutputEvent OnAssignedOutput(RoomAssignedOutputEvent e) => AssignedOutput.InvokeSafely(e);
    public static Event<RoomAssigningOutputEvent> AssigningOutput { get; set; } = new();
    public static RoomAssigningOutputEvent OnAssigningOutput(RoomAssigningOutputEvent e) => AssigningOutput.InvokeSafely(e);
    public static Event<RoomConstructedEvent> Constructed { get; set; } = new();
    public static RoomConstructedEvent OnConstructed(RoomConstructedEvent e) => Constructed.InvokeSafely(e);
    public static Event<RoomConstructingEvent> Constructing { get; set; } = new();
    public static RoomConstructingEvent OnConstructing(RoomConstructingEvent e) => Constructing.InvokeSafely(e);
    public static Event<RoomProcessingIncreasedEvent> ProcessingIncreased { get; set; } = new();
    public static RoomProcessingIncreasedEvent OnProcessingIncreased(RoomProcessingIncreasedEvent e) => ProcessingIncreased.InvokeSafely(e);
    public static Event<RoomProcessingIncreasingEvent> ProcessingIncreasing { get; set; } = new();
    public static RoomProcessingIncreasingEvent OnProcessingIncreasing(RoomProcessingIncreasingEvent e) => ProcessingIncreasing.InvokeSafely(e);
    public static Event<RoomConsumedResourcesEvent> ConsumedResources { get; set; } = new();
    public static RoomConsumedResourcesEvent OnConsumedResources(RoomConsumedResourcesEvent e) => ConsumedResources.InvokeSafely(e);
    public static Event<RoomConsumingResourcesEvent> ConsumingResources { get; set; } = new();
    public static RoomConsumingResourcesEvent OnConsumingResources(RoomConsumingResourcesEvent e) => ConsumingResources.InvokeSafely(e);
    public static Event<RoomReceivedResourcesEvent> ReceivedResources { get; set; } = new();
    public static RoomReceivedResourcesEvent OnReceivedResources(RoomReceivedResourcesEvent e) => ReceivedResources.InvokeSafely(e);
    public static Event<RoomReceivingResourcesEvent> ReceivingResources { get; set; } = new();
    public static RoomReceivingResourcesEvent OnReceivingResources(RoomReceivingResourcesEvent e) => ReceivingResources.InvokeSafely(e);
    public static Event<RoomStoppedWorkEvent> StoppedWork { get; set; } = new();
    public static RoomStoppedWorkEvent OnStoppedWork(RoomStoppedWorkEvent e) => StoppedWork.InvokeSafely(e);
    public static Event<RoomStoppingWorkEvent> StoppingWork { get; set; } = new();
    public static RoomStoppingWorkEvent OnStoppingWork(RoomStoppingWorkEvent e) => StoppingWork.InvokeSafely(e);
}