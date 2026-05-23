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

    public IReadonlyNumericDict? EditedData;

    Control? Sources => GetNode("%sources") as Control;
    Button? Collapse => GetNode("%collapse") as Button;
    RichTextLabel? CollapseLabel => GetNode("%collapseLabel") as RichTextLabel;

    public override void _Ready()
    {
        if (Sources is null || Collapse is null)
            return;
        Collapse.Pressed += _onCollapsePressed;
        Sources.Visible = false;
    }

    public void Setup<[MustBeVariant] TKey, [MustBeVariant] TValue>(IReadonlyNumericDict data, System.Collections.Generic.IReadOnlyDictionary<string, TKey> definitions)
        where TKey : Resource
        where TValue : new()
    {
        EditedData = data;

        foreach (var (name, def) in definitions)
        {
            var itemSelector = ResourceLoader.Load<PackedScene>("res://resource_types/scenes/EditorItemSelector.tscn").Instantiate<EditorItemSelector>();
            itemSelector.OnAmountChanged += _onAmountChanged;
            itemSelector.Name = name;
            var value = Variant.From(data.TryGetValueUntyped<TKey, TValue>(def, out var v) ? v : default);
            itemSelector.SetItemDefinition<TValue>(def, value);
            Sources?.AddChild(itemSelector);
        }
        _updateLabel();
    }

    private void _onAmountChanged(Resource def, Variant value)
    {
        if (_testZero(value))
        {
            EmitSignalOnRemove(def);
        }
        else
        {
            EmitSignalOnChange(def, value);
        }
        _updateLabel();
    }

    private static bool _testZero(Variant value) => value.Obj?.Equals(0L) ?? true;

    private void _updateLabel()
    {
        if (CollapseLabel is not null)
        {
            CollapseLabel.Text = EditedData is IToBBCode bbcode ? bbcode.ToStringBBCode() : EditedData?.GetType().Name;
            CollapseLabel.Text = string.IsNullOrWhiteSpace(CollapseLabel.Text) ? "<nothing>" : CollapseLabel.Text;
        }
    }

    private void _onFilterSubmitted(string value)
    {
        value = value.ToLower();
        if (Sources is null)
            return;
        foreach (var selector in Sources.GetChildren())
        {
            if (selector is Control s)
                s.Visible = string.IsNullOrWhiteSpace(value) || s.Name.ToString().Contains(value, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    private void _onCollapsePressed()
    {
        if (Sources is null)
            return;
        Sources.Visible = !Sources.Visible;
    }
}
