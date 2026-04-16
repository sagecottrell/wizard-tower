using Godot;
using wizardtower.events.interfaces;

namespace wizardtower.events;

public abstract partial class BaseEvent : GodotObject, IEvent
{
    public IEvent? Source { get; set; }
}
