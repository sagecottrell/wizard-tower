using Godot;
using wizardtower.events.interfaces;

namespace wizardtower.events;

public abstract partial class BaseEvent : GodotObject, IEvent
{
    public IEvent? Source { get; set; }

    /// <summary>
    /// the input event that triggered this chain of events
    /// </summary>
    public InputEvent? Input { get; set; }
}

public static class BaseEventExtensions
{
    public static T CopySourceInput<T>(this T @event, IEvent source) where T : BaseEvent
    {
        @event.Source = source;
        @event.Input = source.Input;
        return @event;
    }
}