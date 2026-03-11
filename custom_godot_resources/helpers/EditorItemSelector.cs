using Godot;

namespace wizardtower.custom_godot_resources.helpers;

[Tool]
[GlobalClass]
public partial class EditorItemSelector : Control
{
    private ItemDefinition? _itemDef;

    [Signal]
    public delegate void OnAmountChangedEventHandler(ItemDefinition def, int value);

    public void SetItemDefinition(ItemDefinition def, int amount)
    {
        _itemDef = def;
        GetNode<SpinBox>("SpinBox").Value = amount;
        GetNode<Label>("Label").Text = def.ItemName;
        GetNode<TextureRect>("TextureRect").Texture = def.Icon;
    }

    public override void _Ready()
    {
        GetNode<SpinBox>("SpinBox").ValueChanged += _onValueChanged;
    }

    private void _onValueChanged(double value)
    {
        EmitSignalOnAmountChanged(_itemDef, (int)value);
    }
}
