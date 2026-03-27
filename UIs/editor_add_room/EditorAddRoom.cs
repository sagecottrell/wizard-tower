using Godot;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.editor_add_room;

[Tool]
public partial class EditorAddRoom : Control
{
    [Signal]
    public delegate void OnSaveEventHandler(int elevation, int floorPosition, RoomDefinition def);

    [Export]
    public TowerState? TowerState { get; set; }

    [Export]
    public CheckBox roomPickerPrefab { get; set; }

    [Export]
    public SpinBox ev { get; set; }

    [Export]
    public SpinBox fp { get; set; }

    public ButtonGroup group { get; set; } = new();
    public RoomDefinition? pickedRoom { get; set; }

    public override void _Ready()
    {
        foreach (var (name, def) in LoadDefs.LoadAll<RoomDefinition>())
        {
            if (roomPickerPrefab.Duplicate() is CheckBox dup)
            {
                dup.ButtonGroup = group;
                dup.Text = name;
                dup.Pressed += () => {
                    pickedRoom = def;
                };
                roomPickerPrefab.GetParent().AddChild(dup);
            }
        }

        roomPickerPrefab.Visible = false;
    }

    public override void _Process(double delta)
    {
        if (TowerState is null)
            return;
        ev.MinValue = -TowerState.MaxBasement;
        ev.MaxValue = TowerState.MaxHeight;
        fp.MinValue = -TowerState.MaxWidth;
        fp.MaxValue = TowerState.MaxWidth;
    }

    public void Save()
    {
        EmitSignalOnSave((int)ev.Value, (int)fp.Value, pickedRoom);
    }
}
