using Godot;
using Godot.Collections;
using wizardtower.resource_types;

namespace wizardtower.state.room_functions;

[Tool]
[GlobalClass]
public partial class RoomConvertResourcesState : Resource
{
    [Export]
    public uint TimesProducedToday { get; set; } = 0;

    [Export]
    public double ProductionProgress { get; set; } = 0;

    [Export]
    public bool CurrentlyWorking { get; set; }

    [Export]
    public uint WorkersPresent { get; set; }

    [Export]
    public Array<uint>? RoomsSupposedToSendResourcesHere { get; set; }

    [Export]
    public RecipeDefinition? SelectedRecipe { get; set; }
}
