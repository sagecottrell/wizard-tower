using wizardtower.events.interfaces;

namespace wizardtower.events.ui;

public partial class CancelledUIEvent() : BaseEvent, IAllowableEvent
{
    public bool IsAllowed { get; set; } = true;
}
