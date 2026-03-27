using Godot;
using wizardtower.resource_types;

namespace wizardtower.UIs.build_menu;

public partial class BuildButton : HBoxContainer
{
    [Signal]
    public delegate void OnClickedEventHandler(BuildButton btn);

    public RoomDefinition? RoomDefinition;
    public FloorDefinition? FloorDefinition;
    public TransportDefinition? TransportDefinition;

    public override void _Ready()
    {
        MouseDefaultCursorShape = CursorShape.PointingHand;
        GuiInput += _buildButton_GuiInput;
    }

    private void _buildButton_GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mev && mev.ButtonMask == MouseButtonMask.Left)
        {
            if (mev.Pressed)
            {
                EmitSignalOnClicked(this);
            }
        }
    }
}
