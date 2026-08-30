
using System.Diagnostics.CodeAnalysis;
using wizardtower.events.features;
using wizardtower.events.Floor;
using wizardtower.events.Floor.ui;

namespace wizardtower.events.handlers;
    
public static partial class FloorEvents {
    public static partial class Ui {        
        public static Event<FloorConstructionSelectedEvent> ConstructionSelected { get; set; } = new();
        public static FloorConstructionSelectedEvent OnConstructionSelected(FloorConstructionSelectedEvent e) => ConstructionSelected.InvokeSafely(e);
        public static FloorConstructionSelectedEvent OnConstructionSelected(FloorConstructionSelectedEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionSelected.InvokeSafely(e); 
        }        
        public static Event<FloorConstructionSelectingEvent> ConstructionSelecting { get; set; } = new();
        public static FloorConstructionSelectingEvent OnConstructionSelecting(FloorConstructionSelectingEvent e) => ConstructionSelecting.InvokeSafely(e);
        public static FloorConstructionSelectingEvent OnConstructionSelecting(FloorConstructionSelectingEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionSelecting.InvokeSafely(e); 
        }        
        public static Event<FloorConstructionStoppedEvent> ConstructionStopped { get; set; } = new();
        public static FloorConstructionStoppedEvent OnConstructionStopped(FloorConstructionStoppedEvent e) => ConstructionStopped.InvokeSafely(e);
        public static FloorConstructionStoppedEvent OnConstructionStopped(FloorConstructionStoppedEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionStopped.InvokeSafely(e); 
        }        
        public static Event<FloorConstructionStoppingEvent> ConstructionStopping { get; set; } = new();
        public static FloorConstructionStoppingEvent OnConstructionStopping(FloorConstructionStoppingEvent e) => ConstructionStopping.InvokeSafely(e);
        public static FloorConstructionStoppingEvent OnConstructionStopping(FloorConstructionStoppingEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionStopping.InvokeSafely(e); 
        }        
        public static Event<FloorExtensionShowedEvent> ExtensionShowed { get; set; } = new();
        public static FloorExtensionShowedEvent OnExtensionShowed(FloorExtensionShowedEvent e) => ExtensionShowed.InvokeSafely(e);
        public static FloorExtensionShowedEvent OnExtensionShowed(FloorExtensionShowedEvent e, BaseEvent source) { 
            e.Source = source; 
            return ExtensionShowed.InvokeSafely(e); 
        }        
        public static Event<FloorExtensionShowingEvent> ExtensionShowing { get; set; } = new();
        public static FloorExtensionShowingEvent OnExtensionShowing(FloorExtensionShowingEvent e) => ExtensionShowing.InvokeSafely(e);
        public static FloorExtensionShowingEvent OnExtensionShowing(FloorExtensionShowingEvent e, BaseEvent source) { 
            e.Source = source; 
            return ExtensionShowing.InvokeSafely(e); 
        }        
        public static bool TryConstructionSelecting(FloorConstructionSelectingEvent pre, [NotNullWhen(true)] out FloorConstructionSelectedEvent? e) {
            e = null;
            if (OnConstructionSelecting(pre).IsAllowed) {
                e = pre.Into();
                return true;
            }
            return false;
        }        
        public static bool TryConstructionStopping(FloorConstructionStoppingEvent pre, [NotNullWhen(true)] out FloorConstructionStoppedEvent? e) {
            e = null;
            if (OnConstructionStopping(pre).IsAllowed) {
                e = pre.Into();
                return true;
            }
            return false;
        }        
        public static bool TryExtensionShowing(FloorExtensionShowingEvent pre, [NotNullWhen(true)] out FloorExtensionShowedEvent? e) {
            e = null;
            if (OnExtensionShowing(pre).IsAllowed) {
                e = pre.Into();
                return true;
            }
            return false;
        }
    }
}