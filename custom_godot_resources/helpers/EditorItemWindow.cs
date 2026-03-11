using Godot;
using System.Collections.Generic;

namespace wizardtower.custom_godot_resources.helpers;

[Tool]
[GlobalClass]
public partial class EditorItemWindow : Control
{
    [Signal]
    public delegate void OnSaveEventHandler(Godot.Collections.Dictionary<ItemDefinition, int> data);

    public Dictionary<ItemDefinition, int> EditedData = [];

    public override void _Ready()
    {
        GetNode<LineEdit>("%filter").TextChanged += _onFilterSubmitted;
        GetNode<Button>("%save").Pressed += _onSaveButtonPressed;
        foreach (var (name, def) in ItemDefinition.AllDefinitions)
        {
            var itemSelector = ResourceLoader.Load<PackedScene>("res://custom_godot_resources/scenes/EditorItemSelector.tscn").Instantiate<EditorItemSelector>();
            itemSelector.OnAmountChanged += _onAmountChanged;
            var value = EditedData.GetValueOrDefault(def, 0);
            itemSelector.SetItemDefinition(def, value);
            GetNode("%sources").AddChild(itemSelector);

            if (value > 0) {
                _updateLabel(def, EditedData.GetValueOrDefault(def, 0));
            }
        }
    }

    private void _onAmountChanged(ItemDefinition def, int value)
    {
        if (value > 0)
        {
            EditedData[def] = value;
        }
        else
        {
            EditedData.Remove(def);
        }
        _updateLabel(def, value);
    }

    private void _updateLabel(ItemDefinition def, int value)
    {
        if (GetNode("%selected").FindChild(def.ItemName, owned: false) is Label label)
        {
            if (value <= 0)
                label.QueueFree();
            else
                label.Text = _formatLabel(def, value);
        }
        else
        {
            GetNode("%selected").AddChild(new Label()
            {
                Name = def.ItemName,
                Text = _formatLabel(def, value),
            });
        }
    }

    private static string _formatLabel(ItemDefinition def, int amount) => $"{def.ItemName}: {amount}";

    private void _onFilterSubmitted(string value)
    {
        foreach (var selector in GetNode("%sources").GetChildren())
        {
            if (selector is Control s)
                s.Visible = string.IsNullOrWhiteSpace(value) || s.Name.ToString().Contains(value);
        }
    }

    private void _onSaveButtonPressed()
    {
        EmitSignalOnSave(new(EditedData));
    }
}
