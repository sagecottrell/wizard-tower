
using wizardtower.events.features;
using wizardtower.events.Room;
using wizardtower.events.Room.ui;

namespace wizardtower.events.handlers;
    
public static partial class RoomEvents {
    public static partial class Ui {
        public static Event<RoomConstructionSelectedEvent> ConstructionSelected { get; set; } = new();
        public static RoomConstructionSelectedEvent OnConstructionSelected(RoomConstructionSelectedEvent e) => ConstructionSelected.InvokeSafely(e);
        public static Event<RoomConstructionSelectingEvent> ConstructionSelecting { get; set; } = new();
        public static RoomConstructionSelectingEvent OnConstructionSelecting(RoomConstructionSelectingEvent e) => ConstructionSelecting.InvokeSafely(e);
        public static Event<RoomSelectedEvent> Selected { get; set; } = new();
        public static RoomSelectedEvent OnSelected(RoomSelectedEvent e) => Selected.InvokeSafely(e);
        public static Event<RoomSelectingEvent> Selecting { get; set; } = new();
        public static RoomSelectingEvent OnSelecting(RoomSelectingEvent e) => Selecting.InvokeSafely(e);
        public static Event<RoomConstructionPreviewStartedEvent> ConstructionPreviewStarted { get; set; } = new();
        public static RoomConstructionPreviewStartedEvent OnConstructionPreviewStarted(RoomConstructionPreviewStartedEvent e) => ConstructionPreviewStarted.InvokeSafely(e);
        public static Event<RoomConstructionPreviewStartingEvent> ConstructionPreviewStarting { get; set; } = new();
        public static RoomConstructionPreviewStartingEvent OnConstructionPreviewStarting(RoomConstructionPreviewStartingEvent e) => ConstructionPreviewStarting.InvokeSafely(e);
        public static Event<RoomConstructionPreviewStoppedEvent> ConstructionPreviewStopped { get; set; } = new();
        public static RoomConstructionPreviewStoppedEvent OnConstructionPreviewStopped(RoomConstructionPreviewStoppedEvent e) => ConstructionPreviewStopped.InvokeSafely(e);
        public static Event<RoomConstructionPreviewStoppingEvent> ConstructionPreviewStopping { get; set; } = new();
        public static RoomConstructionPreviewStoppingEvent OnConstructionPreviewStopping(RoomConstructionPreviewStoppingEvent e) => ConstructionPreviewStopping.InvokeSafely(e);
        public static Event<RoomConstructionStoppedEvent> ConstructionStopped { get; set; } = new();
        public static RoomConstructionStoppedEvent OnConstructionStopped(RoomConstructionStoppedEvent e) => ConstructionStopped.InvokeSafely(e);
        public static Event<RoomConstructionStoppingEvent> ConstructionStopping { get; set; } = new();
        public static RoomConstructionStoppingEvent OnConstructionStopping(RoomConstructionStoppingEvent e) => ConstructionStopping.InvokeSafely(e);
        public static Event<RoomDeselectedEvent> Deselected { get; set; } = new();
        public static RoomDeselectedEvent OnDeselected(RoomDeselectedEvent e) => Deselected.InvokeSafely(e);
        public static Event<RoomDeselectingEvent> Deselecting { get; set; } = new();
        public static RoomDeselectingEvent OnDeselecting(RoomDeselectingEvent e) => Deselecting.InvokeSafely(e);
    }
}