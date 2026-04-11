using Godot;

namespace wizardtower.UIs.build_menu;

public partial class RoomSelected : Node3D
{
    [Signal]
    public delegate void OnAcceptEventHandler(RoomSelected self);
    [Signal]
    public delegate void OnCancelEventHandler(RoomSelected self);
    [Signal]
    public delegate void OnMouseEnteredEventHandler(RoomSelected self);
    [Signal]
    public delegate void OnMouseExitedEventHandler(RoomSelected self);

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
