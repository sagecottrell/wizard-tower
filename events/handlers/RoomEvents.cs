using wizardtower.events.features;
using wizardtower.events.ui;

namespace wizardtower.events.handlers;

public static class RoomEvents
{
    public static class UI
    {
        public static Event<RoomConstructionPreviewEvent> RoomConstructionPreview { get; set; } = new();
        public static Event<RoomConstructionPreviewStoppedEvent> RoomConstructionPreviewStopped { get; set; } = new();
        public static Event<RoomConstructionSelectingEvent> RoomConstructionSelecting { get; set; } = new();
        public static Event<RoomConstructionSelectedEvent> RoomConstructionSelected { get; set; } = new();
        public static Event<RoomConstructionStoppingEvent> RoomConstructionStopping { get; set; } = new();
        public static Event<RoomConstructionStoppedEvent> RoomConstructionStopped { get; set; } = new();
        public static Event<RoomSelectingEvent> RoomSelecting { get; set; } = new();
        public static Event<RoomSelectedEvent> RoomSelected { get; set; } = new();
        public static Event<RoomDeselectingEvent> RoomDeselecting { get; set; } = new();
        public static Event<RoomDeselectedEvent> RoomDeselected { get; set; } = new();
        public static RoomConstructionPreviewEvent OnRoomConstructionPreview(RoomConstructionPreviewEvent e) => RoomConstructionPreview.InvokeSafely(e);
        public static RoomConstructionPreviewStoppedEvent OnRoomConstructionPreviewStopped(RoomConstructionPreviewStoppedEvent e) => RoomConstructionPreviewStopped.InvokeSafely(e);
        public static RoomConstructionSelectingEvent OnRoomConstructionSelecting(RoomConstructionSelectingEvent e) => RoomConstructionSelecting.InvokeSafely(e);
        public static RoomConstructionSelectedEvent OnRoomConstructionSelected(RoomConstructionSelectedEvent e) => RoomConstructionSelected.InvokeSafely(e);
        public static RoomConstructionStoppingEvent OnRoomConstructionStopping(RoomConstructionStoppingEvent e) => RoomConstructionStopping.InvokeSafely(e);
        public static RoomConstructionStoppedEvent OnRoomConstructionStopped(RoomConstructionStoppedEvent e) => RoomConstructionStopped.InvokeSafely(e);
        public static RoomSelectingEvent OnRoomSelecting(RoomSelectingEvent e) => RoomSelecting.InvokeSafely(e);
        public static RoomSelectedEvent OnRoomSelected(RoomSelectedEvent e) => RoomSelected.InvokeSafely(e);
        public static RoomDeselectingEvent OnRoomDeselecting(RoomDeselectingEvent e) => RoomDeselecting.InvokeSafely(e);
        public static RoomDeselectedEvent OnRoomDeselected(RoomDeselectedEvent e) => RoomDeselected.InvokeSafely(e);
    }

    public static Event<RoomConstructingEvent> RoomConstructing { get; set; } = new();
    public static Event<RoomConstructedEvent> RoomConstructed { get; set; } = new();

    public static Event<RoomDestroyingEvent> RoomDestroying { get; set; } = new();
    public static Event<RoomDestroyedEvent> RoomDestroyed { get; set; } = new();

    public static RoomConstructingEvent OnRoomConstructing(RoomConstructingEvent e) => RoomConstructing.InvokeSafely(e);
    public static RoomConstructedEvent OnRoomConstructed(RoomConstructedEvent e) => RoomConstructed.InvokeSafely(e);

    public static RoomDestroyingEvent OnRoomDestroying(RoomDestroyingEvent e) => RoomDestroying.InvokeSafely(e);
    public static RoomDestroyedEvent OnRoomDestroyed(RoomDestroyedEvent e) => RoomDestroyed.InvokeSafely(e);
}
