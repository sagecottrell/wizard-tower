
using wizardtower.events.features;
using wizardtower.events.Floor;

namespace wizardtower.events.handlers;
    
public static partial class FloorEvents {
    public static Event<FloorExtendedEvent> Extended { get; set; } = new();
    public static FloorExtendedEvent OnExtended(FloorExtendedEvent e) => Extended.InvokeSafely(e);
    public static FloorExtendedEvent OnExtended(FloorExtendedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Extended.InvokeSafely(e); 
    }

    public static Event<FloorExtendingEvent> Extending { get; set; } = new();
    public static FloorExtendingEvent OnExtending(FloorExtendingEvent e) => Extending.InvokeSafely(e);
    public static FloorExtendingEvent OnExtending(FloorExtendingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Extending.InvokeSafely(e); 
    }

    public static Event<FloorConstructedEvent> Constructed { get; set; } = new();
    public static FloorConstructedEvent OnConstructed(FloorConstructedEvent e) => Constructed.InvokeSafely(e);
    public static FloorConstructedEvent OnConstructed(FloorConstructedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Constructed.InvokeSafely(e); 
    }

    public static Event<FloorConstructingEvent> Constructing { get; set; } = new();
    public static FloorConstructingEvent OnConstructing(FloorConstructingEvent e) => Constructing.InvokeSafely(e);
    public static FloorConstructingEvent OnConstructing(FloorConstructingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Constructing.InvokeSafely(e); 
    }

    public static Event<FloorReplacedEvent> Replaced { get; set; } = new();
    public static FloorReplacedEvent OnReplaced(FloorReplacedEvent e) => Replaced.InvokeSafely(e);
    public static FloorReplacedEvent OnReplaced(FloorReplacedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Replaced.InvokeSafely(e); 
    }

    public static Event<FloorReplacingEvent> Replacing { get; set; } = new();
    public static FloorReplacingEvent OnReplacing(FloorReplacingEvent e) => Replacing.InvokeSafely(e);
    public static FloorReplacingEvent OnReplacing(FloorReplacingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Replacing.InvokeSafely(e); 
    }

}