using Godot;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/research-icon.svg")]
[GlobalClass]
public partial class AccountUpgradeDefinition : Resource, INamedResource<AccountUpgradeDefinition>
{
    [Export]
    public string? Name { get; set; }

    [Export]
    public Texture2D? Icon { get; set; }

    [Export]
    public string? Readme { get; set; }

    [Export]
    public string? Description { get; set; }

    [Export]
    public NumericDict<ItemDefinition, uint>? CostToResearch { get; set; } = [];
}
