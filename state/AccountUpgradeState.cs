using Godot;
using wizardtower.resource_types;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class AccountUpgradeState : Resource
{
    [Export]
    public AccountUpgradeDefinition? Definition { get; set; }
}
