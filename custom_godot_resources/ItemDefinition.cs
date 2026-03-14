using Godot;
using System.Collections.Generic;
using wizardtower.custom_godot_resources.containers;

namespace wizardtower.custom_godot_resources;

[Tool]
[Icon("res://custom_godot_resources/item-icon.svg")]
[GlobalClass]
public partial class ItemDefinition : Resource, INamedResource
{
    [Export]
    public string Name { get; set; } = "New Item";
    [Export]
    public string Description { get; set; } = "Item description.";
    [Export]
    public Texture2D? Icon { get; set; }

    [DefinitionLoader]
    public static Dictionary<string, ItemDefinition> AllDefinitions => LoadDefs.LoadAll<ItemDefinition>("res://items/", r => r.Name);
}
