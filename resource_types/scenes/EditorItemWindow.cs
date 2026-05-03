using Godot;
using Godot.Collections;
using System;

namespace wizardtower.resource_types.scenes;

[Tool]
[GlobalClass]
public partial class EditorItemWindow : Control
{
    [Signal]
    public delegate void OnChangeEventHandler(Variant key, Variant value);

    [Signal]
    public delegate void OnRemoveEventHandler(Variant key);

    public Dictionary EditedData = [];

    Control Sources => GetNode("%sources") as Control;
    Button Collapse => GetNode("%collapse") as Button;

    public override void _Ready()
    {
        Collapse.Pressed += _onCollapsePressed;
    }

    public void Setup<[MustBeVariant] TKey, [MustBeVariant] TValue>(Dictionary<TKey, TValue> data, System.Collections.Generic.IReadOnlyDictionary<string, TKey> definitions)
        where TKey : Resource
        where TValue : new()
    {
        EditedData = (Dictionary)data.Duplicate();

        foreach (var (name, def) in definitions)
        {
            var itemSelector = ResourceLoader.Load<PackedScene>("res://resource_types/scenes/EditorItemSelector.tscn").Instantiate<EditorItemSelector>();
            itemSelector.OnAmountChanged += _onAmountChanged;
            itemSelector.Name = name;
            var value = Variant.From(data.TryGetValue(def, out var v) ? v : default);
            itemSelector.SetItemDefinition<TValue>(def, value);
            Sources.AddChild(itemSelector);
        }
    }

    private void _onAmountChanged(Resource def, Variant value)
    {
        if (_testZero(value))
        {
            EditedData.Remove(def);
            EmitSignalOnRemove(def);
        }
        else
        {
            EditedData[def] = value;
            EmitSignalOnChange(def, value);
        }
    }

    //private static string _formatLabel(INamedResource def, Variant amount) => $"[img height=24]{def?.IconPathOrName}[/img] {def?.Name}: {(amount.Obj is IToBBCode bbcode ? bbcode.ToStringBBCode() : amount)}";

    private static bool _testZero(Variant value) => value.Obj?.Equals(0L) ?? true;

    private void _onFilterSubmitted(string value)
    {
        value = value.ToLower();
        foreach (var selector in Sources.GetChildren())
        {
            if (selector is Control s)
                s.Visible = string.IsNullOrWhiteSpace(value) || s.Name.ToString().Contains(value, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    private void _onCollapsePressed()
    {
        Sources.Visible = !Sources.Visible;
    }
}
