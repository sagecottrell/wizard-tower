using Godot;

namespace wizardtower.state.room_functions;

[Tool]
[GlobalClass]
public partial class RoomConvertResourcesState : BaseRoomFunctionState
{
    [Export]
    public uint TimesProducedToday { get; set; } = 0;

    [Export]
    public double ProductionProgress { get; set; } = 0;

    [Export]
    public bool CurrentlyWorking { get; set; }

    [Export]
    public uint WorkersPresent { get; set; }
}
