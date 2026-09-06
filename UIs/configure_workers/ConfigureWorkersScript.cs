using Godot;
using wizardtower.state;

namespace wizardtower.UIs.configure_workers;

public partial class ConfigureWorkersScript(TowerState tower, RoomState room) : Node3D
{
    public TowerState Tower { get; } = tower;
    public RoomState Room { get; } = room;

    ConfigureWorkersUI ui = new(tower, room);

    public override void _Ready()
    {
        Name = nameof(ConfigureWorkersScript);
        AddChild(new PanelContainer()
        {
            AnchorRight = 1,
            AnchorLeft = 1,
            PivotOffsetRatio = new Vector2(1, 0),
            GrowHorizontal = Control.GrowDirection.Begin,
        }.WithChild(ui));
    }

    
}
