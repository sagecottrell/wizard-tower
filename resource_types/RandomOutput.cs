using Godot;

namespace wizardtower.resource_types;

[Tool]
[GlobalClass]
public partial class RandomOutput : Resource
{
    [Export]
    public int Weight { get; set; }

    [Export]
    public NumericDict<ItemDefinition, uint> Output { get; set; } = [];
}
