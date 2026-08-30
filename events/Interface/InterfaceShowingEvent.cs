/**
Generated from ./events/Interface/InterfaceShowedEvent.cs
**/

using wizardtower.events.interfaces;

namespace wizardtower.events.Interface;

public partial class InterfaceShowingEvent(IUserInterface ui) : BaseEvent, IDeniableEvent, IUserInterfaceEvent
{
    public bool IsAllowed { get; set; } = true;
    public IUserInterface UserInterface { get; set; } = ui;
}


public static class InterfaceShowedEventExtensions {
    public static InterfaceShowedEvent Into(this InterfaceShowingEvent old) {
        return new(ui: old.UserInterface) { Source = old, };
    }
}