
using wizardtower.events.features;
using wizardtower.events.Interface;

namespace wizardtower.events.handlers;
    
public static partial class InterfaceEvents {
    public static Event<InterfaceHidedEvent> Hided { get; set; } = new();
    public static InterfaceHidedEvent OnHided(InterfaceHidedEvent e) => Hided.InvokeSafely(e);
    public static InterfaceHidedEvent OnHided(InterfaceHidedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Hided.InvokeSafely(e); 
    }

    public static Event<InterfaceHidingEvent> Hiding { get; set; } = new();
    public static InterfaceHidingEvent OnHiding(InterfaceHidingEvent e) => Hiding.InvokeSafely(e);
    public static InterfaceHidingEvent OnHiding(InterfaceHidingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Hiding.InvokeSafely(e); 
    }

    public static Event<InterfaceShowedEvent> Showed { get; set; } = new();
    public static InterfaceShowedEvent OnShowed(InterfaceShowedEvent e) => Showed.InvokeSafely(e);
    public static InterfaceShowedEvent OnShowed(InterfaceShowedEvent e, BaseEvent source) { 
        e.Source = source; 
        return Showed.InvokeSafely(e); 
    }

    public static Event<InterfaceShowingEvent> Showing { get; set; } = new();
    public static InterfaceShowingEvent OnShowing(InterfaceShowingEvent e) => Showing.InvokeSafely(e);
    public static InterfaceShowingEvent OnShowing(InterfaceShowingEvent e, BaseEvent source) { 
        e.Source = source; 
        return Showing.InvokeSafely(e); 
    }

}