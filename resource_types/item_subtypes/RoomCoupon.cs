using Godot;

namespace wizardtower.resource_types.item_subtypes;

[Tool]
[Icon("res://resource_types/item-icon.svg")]
[GlobalClass]
public partial class RoomCoupon : ItemDefinition
{
    [Export]
    public RoomDefinition? For { get; set; }

    [Export]
    public override Texture2D? Icon { get; set; }
}
