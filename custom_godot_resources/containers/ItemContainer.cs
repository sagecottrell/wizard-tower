using Godot;

namespace wizardtower.custom_godot_resources.containers;

[Tool]
[Icon("res://custom_godot_resources/item-container-icon.svg")]
[GlobalClass]
public partial class ItemContainer : GenericNumericContainer<ItemContainer, ItemDefinition, int>
{
    protected override ItemDefinition LoadKey(Variant variant) => ItemDefinition.AllDefinitions[(string)variant];
}
