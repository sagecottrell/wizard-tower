using wizardtower.events.features;
using wizardtower.events.Floor;
using wizardtower.events.Floor.ui;

namespace wizardtower.events.handlers;

public static class FloorEvents
{
    public static class UI
    {
        public static Event<FloorConstructionSelectingEvent> ConstructionSelecting { get; set; } = new();
        public static Event<FloorConstructionSelectedEvent> ConstructionSelected { get; set; } = new();

        public static Event<FloorConstructionStoppingEvent> ConstructionStopping { get; set; } = new();
        public static Event<FloorConstructionStoppedEvent> ConstructionStopped { get; set; } = new();

        public static FloorConstructionSelectingEvent OnConstructionSelecting(FloorConstructionSelectingEvent e) => ConstructionSelecting.InvokeSafely(e);
        public static FloorConstructionSelectedEvent OnConstructionSelected(FloorConstructionSelectedEvent e) => ConstructionSelected.InvokeSafely(e);
        public static FloorConstructionStoppingEvent OnConstructionStopping(FloorConstructionStoppingEvent e) => ConstructionStopping.InvokeSafely(e);
        public static FloorConstructionStoppedEvent OnConstructionStopped(FloorConstructionStoppedEvent e) => ConstructionStopped.InvokeSafely(e);
    }

    public static Event<FloorConstructingEvent> Constructing { get; set; } = new();
    public static Event<FloorConstructedEvent> Constructed { get; set; } = new();
    public static Event<FloorExtendingEvent> Extending { get; set; } = new();
    public static Event<FloorExtendedEvent> Extended { get; set; } = new();
    public static Event<FloorReplacingEvent> Replacing { get; set; } = new();
    public static Event<FloorReplacedEvent> Replaced { get; set; } = new();

    public static FloorConstructingEvent OnConstructing(FloorConstructingEvent e) => Constructing.InvokeSafely(e);
    public static FloorConstructedEvent OnConstructed(FloorConstructedEvent e) => Constructed.InvokeSafely(e);
    public static FloorExtendingEvent OnExtending(FloorExtendingEvent e) => Extending.InvokeSafely(e);
    public static FloorExtendedEvent OnExtended(FloorExtendedEvent e) => Extended.InvokeSafely(e);
    public static FloorReplacingEvent OnReplacing(FloorReplacingEvent e) => Replacing.InvokeSafely(e);
    public static FloorReplacedEvent OnReplaced(FloorReplacedEvent e) => Replaced.InvokeSafely(e);
}
