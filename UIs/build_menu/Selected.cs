using Godot;

namespace wizardtower.UIs.build_menu;

public partial class Selected : Node3D
{
    public void InputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            GD.Print("Selected");
        }
    }
}
