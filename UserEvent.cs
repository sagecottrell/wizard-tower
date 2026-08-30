using Godot;
using wizardtower.events.interfaces;

///
/// If present, signifies that the current event chain was started by user input.
/// 
/// Wrapper around godot InputEvent which may or may not have been provided. Most godot input events have an InputEvent, 
/// but some signals like Area3D on mouse entered does not provide an InputEvent.
public sealed partial class UserEvent : RefCounted, IEvent
{
    public InputEvent? GodotInput { get; }
    public UserEvent() : this(null) { }

    public UserEvent(InputEvent? godotInput)
    {
        GodotInput = godotInput;
    }

    public static implicit operator UserEvent(InputEvent godotInput) => new(godotInput);
}