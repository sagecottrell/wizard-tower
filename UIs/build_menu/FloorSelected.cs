using Godot;

namespace wizardtower.UIs.build_menu;

public partial class FloorSelected : Node3D
{
    [Signal]
    public delegate void OnAcceptEventHandler(FloorSelected self);
    [Signal]
    public delegate void OnCancelEventHandler(FloorSelected self);
    [Signal]
    public delegate void OnMouseEnteredEventHandler(FloorSelected self);
    [Signal]
    public delegate void OnMouseExitedEventHandler(FloorSelected self);

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
