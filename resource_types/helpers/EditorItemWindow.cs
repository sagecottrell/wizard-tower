using Godot;
using Godot.Collections;
using System;
using System.Reflection;

namespace wizardtower.resource_types.helpers;

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

    public void Setup<[MustBeVariant] TKey, [MustBeVariant] TValue>(Dictionary<TKey, TValue> data, System.Collections.Generic.IReadOnlyDictionary<string, TKey> definitions) 
        where TKey : Resource
        where TValue : new()
    {
        EditedData = (Dictionary)data.Duplicate();

        if (typeof(TKey).GetCustomAttribute<IconAttribute>() is IconAttribute iconAttribute && ResourceLoader.Singleton.Load(iconAttribute.Path) is Texture2D tx)
            GetNode<LineEdit>("%filter").RightIcon = tx;

        foreach (var (name, def) in definitions)
        {
            var itemSelector = ResourceLoader.Load<PackedScene>("res://resource_types/scenes/EditorItemSelector.tscn").Instantiate<EditorItemSelector>();
            itemSelector.OnAmountChanged += _onAmountChanged;
            itemSelector.Name = name;
            var value = Variant.From(data.TryGetValue(def, out var v) ? v : default);
            itemSelector.SetItemDefinition<TValue>(def, value);
            GetNode("%sources").AddChild(itemSelector);

            if (!_testZero(value))
            {
                _updateLabel(def, value);
            }
        }
    }

    private void _onAmountChanged(Resource def, Variant value)
    {
        if (_testZero(value))
        {
            EditedData.Remove(def);
        }
        else
        {
            EditedData[def] = value;
        }
    }

    private void _updateLabel(Resource def, Variant value)
    {
        if (def is not INamedResource named)
            return;
        if (GetNode("%selected").FindChild(named.Name, owned: false) is RichTextLabel label)
        {
            if (_testZero(value))
                label.QueueFree();
            else
                label.Text = _formatLabel(named, value);
        }
        else
        {
            GetNode("%selected").AddChild(new RichTextLabel()
            {
                FitContent = true,
                BbcodeEnabled = true,
                Name = named.Name ?? "--",
                Text = _formatLabel(named, value),
            });
        }
    }

    public override void _Process(double delta)
    {
        foreach (var key in EditedData.Keys)
        {
            if (key.AsGodotObject() is Resource r && EditedData[key] is Variant v && !_testZero(v))
            {
                _onAmountChanged(r, v);
                _updateLabel(r, v);
            }
        }
    }

    private static string _formatLabel(INamedResource def, Variant amount) => $"[img height=24]{def?.IconPathOrName}[/img] {def?.Name}: {(amount.Obj is IToBBCode bbcode ? bbcode.ToStringBBCode() : amount)}";

    private static bool _testZero(Variant value) => value.Obj?.Equals(0) ?? true;

    private void _onFilterSubmitted(string value)
    {
        value = value.ToLower();
        foreach (var selector in GetNode("%sources").GetChildren())
        {
            if (selector is Control s)
                s.Visible = string.IsNullOrWhiteSpace(value) || s.Name.ToString().Contains(value, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    private void _onSaveButtonPressed()
    {
        EmitSignalOnSave(EditedData);
    }
}
