using Godot;

namespace wizardtower.UIs.build_menu;

public partial class TransportSelected : Node3D
{
    [Signal]
    public delegate void OnAcceptEventHandler(TransportSelected self);
    [Signal]
    public delegate void OnCancelEventHandler(TransportSelected self);
    [Signal]
    public delegate void OnMouseEnteredEventHandler(TransportSelected self);
    [Signal]
    public delegate void OnMouseExitedEventHandler(TransportSelected self);

    public void InputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
    {
        if (@event.IsActionReleased(InputMapConstants.RightClick))
            EmitSignalOnCancel(this);
        if (@event.IsActionReleased(InputMapConstants.LeftClick))
            EmitSignalOnAccept(this);
    }

    public void MouseEntered()
    {
        EmitSignalOnMouseEntered(this);
    }

    public void MouseExited()
    {
        EmitSignalOnMouseExited(this);
    }
}
