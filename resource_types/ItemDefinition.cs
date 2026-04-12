using Godot;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/item-icon.svg")]
[GlobalClass]
[LoadDefinitions("res://items/")]
public partial class ItemDefinition : Resource, INamedResource<ItemDefinition>
{
    [Export]
    public string Name { get; set; } = "New Item";
    [Export]
    public string Description { get; set; } = "Item description.";
    [Export]
    public virtual Texture2D? Icon { get; set; }
    [Export]
    public bool PersistInWallet { get; set; }
}
