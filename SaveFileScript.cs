using Godot;
using Godot.Collections;
using wizardtower.state;

namespace wizardtower;

public partial class SaveFileScript : Node3D
{
    [Export]
    public Array<SaveFileState> DeveloperAvailableSaveStates { get; set; } = [];


}
