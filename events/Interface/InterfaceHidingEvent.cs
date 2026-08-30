/**
Generated from ./events/Interface/InterfaceHidedEvent.cs
**/

using wizardtower.events.interfaces;

namespace wizardtower.events.Interface;

public partial class InterfaceHidingEvent(IUserInterface ui) : BaseEvent, IDeniableEvent, IUserInterfaceEvent
{
    public bool IsAllowed { get; set; } = true;
    public IUserInterface UserInterface { get; set; } = ui;
}


public static class InterfaceHidedEventExtensions {
    public static InterfaceHidedEvent Into(this InterfaceHidingEvent old) {
        return new(ui: old.UserInterface) { Source = old, };
    }
}