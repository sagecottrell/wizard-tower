using Godot;

namespace wizardtower.state;

[Tool]
[GlobalClass]
public partial class FloorState : Resource
{
    [Export]
    public int Elevation { get; set; }

    [Export]
    public uint SizeLeft { get; set; }

    [Export]
    public uint SizeRight { get; set; }
}
