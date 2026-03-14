using Godot;
using Godot.Collections;
using System;

namespace wizardtower.custom_godot_resources.helpers;

[Tool]
[GlobalClass]
public partial class EditorItemWindow : Control
{
    [Signal]
    public delegate void OnSaveEventHandler(Dictionary data);

    public Dictionary EditedData = [];

    public override void _Ready()
    {
        GetNode<LineEdit>("%filter").TextChanged += _onFilterSubmitted;
        GetNode<Button>("%save").Pressed += _onSaveButtonPressed;
    }

    public void Setup<[MustBeVariant] T, [MustBeVariant] TKey>(Dictionary<T, TKey> data, System.Collections.Generic.Dictionary<string, T> definitions) 
        where T : Resource
    {
        EditedData = (Dictionary)data.Duplicate();
        foreach (var (name, def) in definitions)
        {
            var itemSelector = ResourceLoader.Load<PackedScene>("res://custom_godot_resources/scenes/EditorItemSelector.tscn").Instantiate<EditorItemSelector>();
            itemSelector.OnAmountChanged += _onAmountChanged;
            var value = Convert.ToInt32(data.TryGetValue(def, out var v) ? v : 0);
            itemSelector.SetItemDefinition(def, value);
            GetNode("%sources").AddChild(itemSelector);

            if (value > 0)
            {
                _updateLabel(def, value);
            }
        }
    }

    private void _onAmountChanged(Resource def, int value)
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

    private void _updateLabel(Resource def, int value)
    {
        if (def is not INamedResource named)
            return;
        if (GetNode("%selected").FindChild(named.Name, owned: false) is Label label)
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
                Name = named.Name ?? "--",
                Text = _formatLabel(def, value),
            });
        }
    }

    private static string _formatLabel(Resource def, int amount) => $"{(def as INamedResource)?.Name}: {amount}";

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
        EmitSignalOnSave(EditedData);
    }
}
