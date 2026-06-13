using Godot;
using wizardtower.state;

namespace wizardtower.UIs.configure_workers;

public partial class ConfigureWorkersScript(TowerState tower, RoomState room) : Node3D
{
    public TowerState Tower { get; } = tower;
    public RoomState Room { get; } = room;


}
