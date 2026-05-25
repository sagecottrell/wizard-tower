using wizardtower.events.features;
using wizardtower.events.Room;
using wizardtower.events.Room.ui;

namespace wizardtower.events.handlers;

public static class RoomEvents
{
    public static class UI
    {
        public static Event<RoomConstructionPreviewEvent> ConstructionPreview { get; set; } = new();
        public static Event<RoomConstructionPreviewStoppedEvent> ConstructionPreviewStopped { get; set; } = new();
        public static Event<RoomConstructionSelectingEvent> ConstructionSelecting { get; set; } = new();
        public static Event<RoomConstructionSelectedEvent> ConstructionSelected { get; set; } = new();
        public static Event<RoomConstructionStoppingEvent> ConstructionStopping { get; set; } = new();
        public static Event<RoomConstructionStoppedEvent> ConstructionStopped { get; set; } = new();
        public static Event<RoomSelectingEvent> Selecting { get; set; } = new();
        public static Event<RoomSelectedEvent> Selected { get; set; } = new();
        public static Event<RoomDeselectingEvent> Deselecting { get; set; } = new();
        public static Event<RoomDeselectedEvent> Deselected { get; set; } = new();
        public static RoomConstructionPreviewEvent OnConstructionPreview(RoomConstructionPreviewEvent e) => ConstructionPreview.InvokeSafely(e);
        public static RoomConstructionPreviewStoppedEvent OnConstructionPreviewStopped(RoomConstructionPreviewStoppedEvent e) => ConstructionPreviewStopped.InvokeSafely(e);
        public static RoomConstructionSelectingEvent OnConstructionSelecting(RoomConstructionSelectingEvent e) => ConstructionSelecting.InvokeSafely(e);
        public static RoomConstructionSelectedEvent OnConstructionSelected(RoomConstructionSelectedEvent e) => ConstructionSelected.InvokeSafely(e);
        public static RoomConstructionStoppingEvent OnConstructionStopping(RoomConstructionStoppingEvent e) => ConstructionStopping.InvokeSafely(e);
        public static RoomConstructionStoppedEvent OnConstructionStopped(RoomConstructionStoppedEvent e) => ConstructionStopped.InvokeSafely(e);
        public static RoomSelectingEvent OnSelecting(RoomSelectingEvent e) => Selecting.InvokeSafely(e);
        public static RoomSelectedEvent OnSelected(RoomSelectedEvent e) => Selected.InvokeSafely(e);
        public static RoomDeselectingEvent OnDeselecting(RoomDeselectingEvent e) => Deselecting.InvokeSafely(e);
        public static RoomDeselectedEvent OnDeselected(RoomDeselectedEvent e) => Deselected.InvokeSafely(e);
    }

    public static Event<RoomConstructingEvent> Constructing { get; set; } = new();
    public static Event<RoomConstructedEvent> Constructed { get; set; } = new();

    public static Event<RoomDestroyingEvent> Destroying { get; set; } = new();
    public static Event<RoomDestroyedEvent> Destroyed { get; set; } = new();

    public static Event<RoomProducingResourcesEvent> ProducingResources { get; set; } = new();
    public static Event<RoomProducedResourcesEvent> ProducedResources { get; set; } = new();

    public static Event<RoomReceivingResourcesEvent> ReceivingResources { get; set; } = new();
    public static Event<RoomReceivedResourcesEvent> ReceivedResources { get; set; } = new();

    public static Event<RoomConsumingResourcesEvent> ConsumingResources { get; set; } = new();
    public static Event<RoomConsumedResourcesEvent> ConsumedResources { get; set; } = new();

    public static Event<RoomStartingWorkEvent> StartingWork { get; set; } = new();
    public static Event<RoomStartedWorkEvent> StartedWork { get; set; } = new();

    public static Event<RoomProcessingIncreasingEvent> ProcessingIncreasing { get; set; } = new();
    public static Event<RoomProcessingIncreasedEvent> ProcessingIncreased { get; set; } = new();

    public static RoomConstructingEvent OnConstructing(RoomConstructingEvent e) => Constructing.InvokeSafely(e);
    public static RoomConstructedEvent OnConstructed(RoomConstructedEvent e) => Constructed.InvokeSafely(e);

    public static RoomDestroyingEvent OnDestroying(RoomDestroyingEvent e) => Destroying.InvokeSafely(e);
    public static RoomDestroyedEvent OnDestroyed(RoomDestroyedEvent e) => Destroyed.InvokeSafely(e);

    public static RoomProducingResourcesEvent OnProducingResources(RoomProducingResourcesEvent e) => ProducingResources.InvokeSafely(e);
    public static RoomProducedResourcesEvent OnProducedResources(RoomProducedResourcesEvent e) => ProducedResources.InvokeSafely(e);

    public static RoomReceivingResourcesEvent OnRoomReceivingResources(RoomReceivingResourcesEvent e) => ReceivingResources.InvokeSafely(e);
    public static RoomReceivedResourcesEvent OnRoomReceivedResources(RoomReceivedResourcesEvent e) => ReceivedResources.InvokeSafely(e);

    public static RoomConsumingResourcesEvent OnConsumingResources(RoomConsumingResourcesEvent e) => ConsumingResources.InvokeSafely(e);
    public static RoomConsumedResourcesEvent OnConsumedResources(RoomConsumedResourcesEvent e) => ConsumedResources.InvokeSafely(e);

    public static RoomStartingWorkEvent OnStartingWork(RoomStartingWorkEvent e) => StartingWork.InvokeSafely(e);
    public static RoomStartedWorkEvent OnStartedWork(RoomStartedWorkEvent e) => StartedWork.InvokeSafely(e);

    public static RoomProcessingIncreasingEvent OnProcessingIncreasing(RoomProcessingIncreasingEvent e) => ProcessingIncreasing.InvokeSafely(e);
    public static RoomProcessingIncreasedEvent OnProcessingIncreased(RoomProcessingIncreasedEvent e) => ProcessingIncreased.InvokeSafely(e);
}
