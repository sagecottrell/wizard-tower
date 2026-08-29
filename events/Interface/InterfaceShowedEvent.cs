using wizardtower.events.interfaces;

namespace wizardtower.events.Interface;

public partial class InterfaceShowedEvent(IUserInterface ui) : BaseEvent, IUserInterfaceEvent
{
    public IUserInterface UserInterface { get; } = ui;
}
