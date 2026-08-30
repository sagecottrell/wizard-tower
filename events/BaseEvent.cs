using wizardtower.events.interfaces;

namespace wizardtower.events;

public abstract class BaseEvent : IEvent
{
    private IEvent? _source;
    public IEvent? Source { get => _source; set { 
        _source = value; 
        if (value is BaseEvent b && b.Input is {} i) 
            Input = i; 
        else if (value is UserEvent {} i2)
            Input = i2;
    } }

    /// <summary>
    /// the input event that triggered this chain of events
    /// </summary>
    public UserEvent? Input { get; private set; }
}
