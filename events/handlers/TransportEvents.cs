using wizardtower.events.features;
using wizardtower.events.Transport;
using wizardtower.events.Transport.ui;

namespace wizardtower.events.handlers;

public static class TransportEvents
{
    public static class UI
    {
        public static Event<TransportConstructionSelectingEvent> TransportConstructionSelecting { get; set; } = new();
        public static Event<TransportConstructionSelectedEvent> TransportConstructionSelected { get; set; } = new();
        public static Event<TransportConstructionStoppingEvent> TransportConstructionStopping { get; set; } = new();
        public static Event<TransportConstructionStoppedEvent> TransportConstructionStopped { get; set; } = new();
        public static Event<TransportSelectingEvent> TransportSelecting { get; set; } = new();
        public static Event<TransportSelectedEvent> TransportSelected { get; set; } = new();
        public static Event<TransportDeselectingEvent> TransportDeselecting { get; set; } = new();
        public static Event<TransportDeselectedEvent> TransportDeselected { get; set; } = new();
        public static Event<TransportConstructionPreviewEvent> TransportConstructionPreview { get; set; } = new();
        public static Event<TransportConstructionPreviewStoppedEvent> TransportConstructionPreviewStopped { get; set; } = new();

        public static TransportConstructionSelectingEvent OnTransportConstructionSelecting(TransportConstructionSelectingEvent e) => TransportConstructionSelecting.InvokeSafely(e);
        public static TransportConstructionSelectedEvent OnTransportConstructionSelected(TransportConstructionSelectedEvent e) => TransportConstructionSelected.InvokeSafely(e);
        public static TransportConstructionStoppingEvent OnTransportConstructionStopping(TransportConstructionStoppingEvent e) => TransportConstructionStopping.InvokeSafely(e);
        public static TransportConstructionStoppedEvent OnTransportConstructionStopped(TransportConstructionStoppedEvent e) => TransportConstructionStopped.InvokeSafely(e);
        public static TransportSelectingEvent OnTransportSelecting(TransportSelectingEvent e) => TransportSelecting.InvokeSafely(e);
        public static TransportSelectedEvent OnTransportSelected(TransportSelectedEvent e) => TransportSelected.InvokeSafely(e);
        public static TransportDeselectingEvent OnTransportDeselecting(TransportDeselectingEvent e) => TransportDeselecting.InvokeSafely(e);
        public static TransportDeselectedEvent OnTransportDeselected(TransportDeselectedEvent e) => TransportDeselected.InvokeSafely(e);
        public static TransportConstructionPreviewEvent OnTransportConstructionPreview(TransportConstructionPreviewEvent e) => TransportConstructionPreview.InvokeSafely(e);
        public static TransportConstructionPreviewStoppedEvent OnTransportConstructionPreviewStopped(TransportConstructionPreviewStoppedEvent e) => TransportConstructionPreviewStopped.InvokeSafely(e);
    }

    public static Event<TransportConstructingEvent> TransportConstructing { get; set; } = new();
    public static Event<TransportConstructedEvent> TransportConstructed { get; set; } = new();
    public static Event<TransportDestroyingEvent> TransportDestroying { get; set; } = new();
    public static Event<TransportDestroyedEvent> TransportDestroyed { get; set; } = new();

    public static TransportConstructingEvent OnTransportConstructing(TransportConstructingEvent e) => TransportConstructing.InvokeSafely(e);
    public static TransportConstructedEvent OnTransportConstructed(TransportConstructedEvent e) => TransportConstructed.InvokeSafely(e);
    public static TransportDestroyingEvent OnTransportDestroying(TransportDestroyingEvent e) => TransportDestroying.InvokeSafely(e);
    public static TransportDestroyedEvent OnTransportDestroyed(TransportDestroyedEvent e) => TransportDestroyed.InvokeSafely(e);
}
