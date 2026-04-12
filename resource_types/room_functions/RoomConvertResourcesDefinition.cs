using Godot;

namespace wizardtower.resource_types.room_functions;

[Tool]
[GlobalClass]
public partial class RoomConvertResourcesDefinition : BaseRoomFunctionDefinition
{
    [Export]
    public RecipeDefinition Recipe { get; set; } = new();

    [Export]
    public bool ToTowerWallet { get; set; }
}
