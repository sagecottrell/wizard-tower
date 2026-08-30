
using wizardtower.events.features;
using wizardtower.events.Transport;

namespace wizardtower.events.handlers;
    
public static partial class TransportEvents {
    public static Event<TransportDestroyedEvent> Destroyed { get; set; } = new();
    public static TransportDestroyedEvent OnDestroyed(TransportDestroyedEvent e) => Destroyed.InvokeSafely(e);
    public static TransportDestroyedEvent OnDestroyed(TransportDestroyedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Destroyed.InvokeSafely(e); 
    }

    public static Event<TransportDestroyingEvent> Destroying { get; set; } = new();
    public static TransportDestroyingEvent OnDestroying(TransportDestroyingEvent e) => Destroying.InvokeSafely(e);
    public static TransportDestroyingEvent OnDestroying(TransportDestroyingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Destroying.InvokeSafely(e); 
    }

    public static Event<TransportConstructedEvent> Constructed { get; set; } = new();
    public static TransportConstructedEvent OnConstructed(TransportConstructedEvent e) => Constructed.InvokeSafely(e);
    public static TransportConstructedEvent OnConstructed(TransportConstructedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Constructed.InvokeSafely(e); 
    }

    public static Event<TransportConstructingEvent> Constructing { get; set; } = new();
    public static TransportConstructingEvent OnConstructing(TransportConstructingEvent e) => Constructing.InvokeSafely(e);
    public static TransportConstructingEvent OnConstructing(TransportConstructingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Constructing.InvokeSafely(e); 
    }

}