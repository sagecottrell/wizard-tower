using wizardtower.events.features;
using wizardtower.events.Floor;
using wizardtower.events.Floor.ui;

namespace wizardtower.events.handlers;

public static class FloorEvents
{
    public static class UI
    {
        public static Event<FloorConstructionSelectingEvent> FloorConstructionSelecting { get; set; } = new();
        public static Event<FloorConstructionSelectedEvent> FloorConstructionSelected { get; set; } = new();

        public static Event<FloorConstructionStoppingEvent> FloorConstructionStopping { get; set; } = new();
        public static Event<FloorConstructionStoppedEvent> FloorConstructionStopped { get; set; } = new();

        public static FloorConstructionSelectingEvent OnFloorConstructionSelecting(FloorConstructionSelectingEvent e) => FloorConstructionSelecting.InvokeSafely(e);
        public static FloorConstructionSelectedEvent OnFloorConstructionSelected(FloorConstructionSelectedEvent e) => FloorConstructionSelected.InvokeSafely(e);
        public static FloorConstructionStoppingEvent OnFloorConstructionStopping(FloorConstructionStoppingEvent e) => FloorConstructionStopping.InvokeSafely(e);
        public static FloorConstructionStoppedEvent OnFloorConstructionStopped(FloorConstructionStoppedEvent e) => FloorConstructionStopped.InvokeSafely(e);
    }

    public static Event<FloorConstructingEvent> FloorConstructing { get; set; } = new();
    public static Event<FloorConstructedEvent> FloorConstructed { get; set; } = new();
    public static Event<FloorExtendingEvent> FloorExtending { get; set; } = new();
    public static Event<FloorExtendedEvent> FloorExtended { get; set; } = new();
    public static Event<FloorReplacingEvent> FloorReplacing { get; set; } = new();
    public static Event<FloorReplacedEvent> FloorReplaced { get; set; } = new();

    public static FloorConstructingEvent OnFloorConstructing(FloorConstructingEvent e) => FloorConstructing.InvokeSafely(e);
    public static FloorConstructedEvent OnFloorConstructed(FloorConstructedEvent e) => FloorConstructed.InvokeSafely(e);
    public static FloorExtendingEvent OnFloorExtending(FloorExtendingEvent e) => FloorExtending.InvokeSafely(e);
    public static FloorExtendedEvent OnFloorExtended(FloorExtendedEvent e) => FloorExtended.InvokeSafely(e);
    public static FloorReplacingEvent OnFloorReplacing(FloorReplacingEvent e) => FloorReplacing.InvokeSafely(e);
    public static FloorReplacedEvent OnFloorReplaced(FloorReplacedEvent e) => FloorReplaced.InvokeSafely(e);
}
