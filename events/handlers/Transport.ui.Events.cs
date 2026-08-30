
using wizardtower.events.features;
using wizardtower.events.Transport;
using wizardtower.events.Transport.ui;

namespace wizardtower.events.handlers;
    
public static partial class TransportEvents {
    public static partial class Ui {
        public static Event<TransportConstructionPreviewStartedEvent> ConstructionPreviewStarted { get; set; } = new();
        public static TransportConstructionPreviewStartedEvent OnConstructionPreviewStarted(TransportConstructionPreviewStartedEvent e) => ConstructionPreviewStarted.InvokeSafely(e);
        public static TransportConstructionPreviewStartedEvent OnConstructionPreviewStarted(TransportConstructionPreviewStartedEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionPreviewStarted.InvokeSafely(e); 
        }

        public static Event<TransportConstructionPreviewStartingEvent> ConstructionPreviewStarting { get; set; } = new();
        public static TransportConstructionPreviewStartingEvent OnConstructionPreviewStarting(TransportConstructionPreviewStartingEvent e) => ConstructionPreviewStarting.InvokeSafely(e);
        public static TransportConstructionPreviewStartingEvent OnConstructionPreviewStarting(TransportConstructionPreviewStartingEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionPreviewStarting.InvokeSafely(e); 
        }

        public static Event<TransportConstructionPreviewStoppedEvent> ConstructionPreviewStopped { get; set; } = new();
        public static TransportConstructionPreviewStoppedEvent OnConstructionPreviewStopped(TransportConstructionPreviewStoppedEvent e) => ConstructionPreviewStopped.InvokeSafely(e);
        public static TransportConstructionPreviewStoppedEvent OnConstructionPreviewStopped(TransportConstructionPreviewStoppedEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionPreviewStopped.InvokeSafely(e); 
        }

        public static Event<TransportConstructionPreviewStoppingEvent> ConstructionPreviewStopping { get; set; } = new();
        public static TransportConstructionPreviewStoppingEvent OnConstructionPreviewStopping(TransportConstructionPreviewStoppingEvent e) => ConstructionPreviewStopping.InvokeSafely(e);
        public static TransportConstructionPreviewStoppingEvent OnConstructionPreviewStopping(TransportConstructionPreviewStoppingEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionPreviewStopping.InvokeSafely(e); 
        }

        public static Event<TransportConstructionStoppedEvent> ConstructionStopped { get; set; } = new();
        public static TransportConstructionStoppedEvent OnConstructionStopped(TransportConstructionStoppedEvent e) => ConstructionStopped.InvokeSafely(e);
        public static TransportConstructionStoppedEvent OnConstructionStopped(TransportConstructionStoppedEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionStopped.InvokeSafely(e); 
        }

        public static Event<TransportConstructionStoppingEvent> ConstructionStopping { get; set; } = new();
        public static TransportConstructionStoppingEvent OnConstructionStopping(TransportConstructionStoppingEvent e) => ConstructionStopping.InvokeSafely(e);
        public static TransportConstructionStoppingEvent OnConstructionStopping(TransportConstructionStoppingEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionStopping.InvokeSafely(e); 
        }

        public static Event<TransportConstructionSelectedEvent> ConstructionSelected { get; set; } = new();
        public static TransportConstructionSelectedEvent OnConstructionSelected(TransportConstructionSelectedEvent e) => ConstructionSelected.InvokeSafely(e);
        public static TransportConstructionSelectedEvent OnConstructionSelected(TransportConstructionSelectedEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionSelected.InvokeSafely(e); 
        }

        public static Event<TransportConstructionSelectingEvent> ConstructionSelecting { get; set; } = new();
        public static TransportConstructionSelectingEvent OnConstructionSelecting(TransportConstructionSelectingEvent e) => ConstructionSelecting.InvokeSafely(e);
        public static TransportConstructionSelectingEvent OnConstructionSelecting(TransportConstructionSelectingEvent e, BaseEvent source) { 
            e.Source = source; 
            return ConstructionSelecting.InvokeSafely(e); 
        }

        public static Event<TransportSelectedEvent> Selected { get; set; } = new();
        public static TransportSelectedEvent OnSelected(TransportSelectedEvent e) => Selected.InvokeSafely(e);
        public static TransportSelectedEvent OnSelected(TransportSelectedEvent e, BaseEvent source) { 
            e.Source = source; 
            return Selected.InvokeSafely(e); 
        }

        public static Event<TransportSelectingEvent> Selecting { get; set; } = new();
        public static TransportSelectingEvent OnSelecting(TransportSelectingEvent e) => Selecting.InvokeSafely(e);
        public static TransportSelectingEvent OnSelecting(TransportSelectingEvent e, BaseEvent source) { 
            e.Source = source; 
            return Selecting.InvokeSafely(e); 
        }

        public static Event<TransportDeselectedEvent> Deselected { get; set; } = new();
        public static TransportDeselectedEvent OnDeselected(TransportDeselectedEvent e) => Deselected.InvokeSafely(e);
        public static TransportDeselectedEvent OnDeselected(TransportDeselectedEvent e, BaseEvent source) { 
            e.Source = source; 
            return Deselected.InvokeSafely(e); 
        }

        public static Event<TransportDeselectingEvent> Deselecting { get; set; } = new();
        public static TransportDeselectingEvent OnDeselecting(TransportDeselectingEvent e) => Deselecting.InvokeSafely(e);
        public static TransportDeselectingEvent OnDeselecting(TransportDeselectingEvent e, BaseEvent source) { 
            e.Source = source; 
            return Deselecting.InvokeSafely(e); 
        }

    }
}