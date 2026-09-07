using Godot;
using wizardtower.state;

namespace wizardtower.UIs.configure_room;

public partial class ConfigureRoomScript(TowerState tower, RoomState room) : Node3D
{
    public TowerState Tower { get; } = tower;
    public RoomState Room { get; } = room;

    ConfigureRoomUI ui = new(tower, room);

    public override void _Ready()
    {
        Name = nameof(ConfigureRoomScript);
        AddChild(new PanelContainer()
        {
            AnchorRight = 1,
            AnchorLeft = 1,
            PivotOffsetRatio = new Vector2(1, 0),
            GrowHorizontal = Control.GrowDirection.Begin,
        }.WithChild(ui));
    }


}
