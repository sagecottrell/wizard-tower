using Godot;
using wizardtower.events.interfaces;

namespace wizardtower.events.ui;

public partial class HiddenUIEvent(IUserInterface ui) : BaseEvent, IUserInterfaceEvent
{
    public IUserInterface UserInterface { get; } = ui;
}
