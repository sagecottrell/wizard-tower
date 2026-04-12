using Godot;

namespace wizardtower.resource_types;

[Tool]
[GlobalClass]
public partial class RandomOutputDefinition : Resource
{
    [Export]
    public int Weight { get; set; } = 1;

    [Export]
    public NumericDict<ItemDefinition, uint> Output { get; set; } = [];
}
