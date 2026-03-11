using Godot;

namespace wizardtower.custom_godot_resources;

[Tool]
[Icon("res://custom_godot_resources/item-container-icon.svg")]
[GlobalClass]
public partial class ItemContainer : GenericNumericContainer<ItemContainer, ItemDefinition, int>
{
    protected override ItemDefinition LoadKey(Variant variant) => ItemDefinition.AllDefinitions[(string)variant];

    [ExportToolButton("Edit Items")]
    private Callable _editItemsButton => Callable.From(_popupEditor);

    private void _saveItems(Godot.Collections.Dictionary<ItemDefinition, int> data)
    {
        _editorWindow?.QueueFree();
        Clear();
        foreach (var kvp in data)
        {
            this[kvp.Key] = kvp.Value;
        }
    }

    private Window? _editorWindow;

    private void _popupEditor()
    {
        _editorWindow = new();
        _editorWindow.CloseRequested += () => _editorWindow.QueueFree();
        EditorInterface.Singleton.PopupDialog(_editorWindow, new(50, 50, 500, 500));
        var editor = ResourceLoader.Load<PackedScene>("res://custom_godot_resources/scenes/EditorItemWindow.tscn").Instantiate<helpers.EditorItemWindow>();
        editor.EditedData = ToDictionary();
        editor.OnSave += _saveItems;
        _editorWindow.AddChild(editor);
    }
}
