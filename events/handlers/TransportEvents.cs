using wizardtower.events.features;
using wizardtower.events.Transport;
using wizardtower.events.Transport.ui;

namespace wizardtower.events.handlers;

public static class TransportEvents
{
    public static class UI
    {
        public static Event<TransportConstructionSelectingEvent> ConstructionSelecting { get; set; } = new();
        public static Event<TransportConstructionSelectedEvent> ConstructionSelected { get; set; } = new();
        public static Event<TransportConstructionStoppingEvent> ConstructionStopping { get; set; } = new();
        public static Event<TransportConstructionStoppedEvent> ConstructionStopped { get; set; } = new();
        public static Event<TransportSelectingEvent> Selecting { get; set; } = new();
        public static Event<TransportSelectedEvent> Selected { get; set; } = new();
        public static Event<TransportDeselectingEvent> Deselecting { get; set; } = new();
        public static Event<TransportDeselectedEvent> Deselected { get; set; } = new();
        public static Event<TransportConstructionPreviewEvent> ConstructionPreview { get; set; } = new();
        public static Event<TransportConstructionPreviewStoppedEvent> ConstructionPreviewStopped { get; set; } = new();

        public static TransportConstructionSelectingEvent OnConstructionSelecting(TransportConstructionSelectingEvent e) => ConstructionSelecting.InvokeSafely(e);
        public static TransportConstructionSelectedEvent OnConstructionSelected(TransportConstructionSelectedEvent e) => ConstructionSelected.InvokeSafely(e);
        public static TransportConstructionStoppingEvent OnConstructionStopping(TransportConstructionStoppingEvent e) => ConstructionStopping.InvokeSafely(e);
        public static TransportConstructionStoppedEvent OnConstructionStopped(TransportConstructionStoppedEvent e) => ConstructionStopped.InvokeSafely(e);
        public static TransportSelectingEvent OnSelecting(TransportSelectingEvent e) => Selecting.InvokeSafely(e);
        public static TransportSelectedEvent OnSelected(TransportSelectedEvent e) => Selected.InvokeSafely(e);
        public static TransportDeselectingEvent OnDeselecting(TransportDeselectingEvent e) => Deselecting.InvokeSafely(e);
        public static TransportDeselectedEvent OnDeselected(TransportDeselectedEvent e) => Deselected.InvokeSafely(e);
        public static TransportConstructionPreviewEvent OnConstructionPreview(TransportConstructionPreviewEvent e) => ConstructionPreview.InvokeSafely(e);
        public static TransportConstructionPreviewStoppedEvent OnConstructionPreviewStopped(TransportConstructionPreviewStoppedEvent e) => ConstructionPreviewStopped.InvokeSafely(e);
    }

    public static Event<TransportConstructingEvent> Constructing { get; set; } = new();
    public static Event<TransportConstructedEvent> Constructed { get; set; } = new();
    public static Event<TransportDestroyingEvent> Destroying { get; set; } = new();
    public static Event<TransportDestroyedEvent> Destroyed { get; set; } = new();

    public static TransportConstructingEvent OnConstructing(TransportConstructingEvent e) => Constructing.InvokeSafely(e);
    public static TransportConstructedEvent OnConstructed(TransportConstructedEvent e) => Constructed.InvokeSafely(e);
    public static TransportDestroyingEvent OnDestroying(TransportDestroyingEvent e) => Destroying.InvokeSafely(e);
    public static TransportDestroyedEvent OnDestroyed(TransportDestroyedEvent e) => Destroyed.InvokeSafely(e);
}
