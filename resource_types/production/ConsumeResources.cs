using Godot;

namespace wizardtower.resource_types.production;

[Tool]
[GlobalClass]
public partial class ConsumeResources : Resource
{
    [Export]
    public NumericDict<ItemDefinition, uint> Input { get; set; } = [];
}
