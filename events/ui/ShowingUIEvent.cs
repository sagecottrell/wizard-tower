using wizardtower.events.interfaces;

namespace wizardtower.events.ui;

public partial class ShowingUIEvent(IUserInterface ui) : BaseEvent, IUserInterfaceEvent, IAllowableEvent
{
    public bool IsAllowed { get; set; } = true;
    public IUserInterface UserInterface { get; } = ui;
}
