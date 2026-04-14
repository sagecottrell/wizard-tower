using Godot;
using wizardtower.events.interfaces;

namespace wizardtower.events.ui;

public partial class CancelledUIEvent() : GodotObject, IEvent, IAllowableEvent
{
    public bool IsAllowed { get; set; } = true;
}
