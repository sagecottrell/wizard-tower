using Godot;

namespace wizardtower.UIs.build_menu;

public partial class Selected : Node3D
{
    [Signal]
    public delegate void OnAcceptEventHandler(Selected self);
    [Signal]
    public delegate void OnCancelEventHandler(Selected self);
    [Signal]
    public delegate void OnMouseEnteredEventHandler(Selected self);
    [Signal]
    public delegate void OnMouseExitedEventHandler(Selected self);

    public void InputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
    {
        switch (@event)
        {
            case InputEventMouseButton mouseEvent when mouseEvent.Pressed:
                {
                    if (mouseEvent.ButtonMask == MouseButtonMask.Left)
                        EmitSignalOnAccept(this);
                    else if (mouseEvent.ButtonMask == MouseButtonMask.Right)
                        EmitSignalOnCancel(this);
                    break;
                }
        }
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
