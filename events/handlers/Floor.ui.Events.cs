
using wizardtower.events.features;
using wizardtower.events.Floor;
using wizardtower.events.Floor.ui;

namespace wizardtower.events.handlers;
    
public static partial class FloorEvents {
    public static partial class Ui {
        public static Event<FloorConstructionSelectedEvent> ConstructionSelected { get; set; } = new();
        public static FloorConstructionSelectedEvent OnConstructionSelected(FloorConstructionSelectedEvent e) => ConstructionSelected.InvokeSafely(e);
        public static Event<FloorConstructionSelectingEvent> ConstructionSelecting { get; set; } = new();
        public static FloorConstructionSelectingEvent OnConstructionSelecting(FloorConstructionSelectingEvent e) => ConstructionSelecting.InvokeSafely(e);
        public static Event<FloorConstructionStoppedEvent> ConstructionStopped { get; set; } = new();
        public static FloorConstructionStoppedEvent OnConstructionStopped(FloorConstructionStoppedEvent e) => ConstructionStopped.InvokeSafely(e);
        public static Event<FloorConstructionStoppingEvent> ConstructionStopping { get; set; } = new();
        public static FloorConstructionStoppingEvent OnConstructionStopping(FloorConstructionStoppingEvent e) => ConstructionStopping.InvokeSafely(e);
    }
}