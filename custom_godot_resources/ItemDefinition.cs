using Godot;
using System.Collections.Generic;

namespace wizardtower.custom_godot_resources;

[Tool]
[Icon("res://custom_godot_resources/item-icon.svg")]
[GlobalClass]
public partial class ItemDefinition : Resource, INamedResource
{
    [Export]
    public string ItemName { get; set; } = "New Item";
    [Export]
    public string Description { get; set; } = "Item description.";
    [Export]
    public Texture2D? Icon { get; set; }


    static Dictionary<string, ItemDefinition>? _allDefinitions;
    public static Dictionary<string, ItemDefinition> AllDefinitions => LoadDefs.LoadAll(ref _allDefinitions, "res://items/", r => r.ItemName);
}
