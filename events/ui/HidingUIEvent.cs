using Godot;
using wizardtower.events.interfaces;

namespace wizardtower.events.ui;

public partial class HidingUIEvent(IUserInterface ui) : BaseEvent, IDeniableEvent, IUserInterfaceEvent
{
    public bool IsAllowed { get; set; } = true;
    public IUserInterface UserInterface { get; } = ui;
}
