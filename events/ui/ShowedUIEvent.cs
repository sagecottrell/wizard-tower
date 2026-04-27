using wizardtower.events.interfaces;

namespace wizardtower.events.ui;

public partial class ShowedUIEvent(IUserInterface ui) : BaseEvent, IUserInterfaceEvent
{
    public IUserInterface UserInterface { get; } = ui;
}
