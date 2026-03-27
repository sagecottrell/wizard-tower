using Godot;
using Godot.Collections;
using wizardtower.state;

namespace wizardtower.UIs.save_file_selector;

public partial class SaveFileSelector : Control
{
    [Export]
    public Array<SaveFileState> DeveloperAvailableSaveStates { get; set; } = [];
}
