using Godot;

namespace wizardtower.events.interfaces;

public interface IEvent
{
    IEvent? Source { get; set; }
    InputEvent? Input { get; set; }
}
