using Godot;

namespace wizardtower.custom_godot_resources.helpers;

[Tool]
[GlobalClass]
public partial class EditorItemSelector : Control
{
    private Resource? _itemDef;

    [Signal]
    public delegate void OnAmountChangedEventHandler(Resource def, int value);

    public void SetItemDefinition(Resource def, int amount)
    {
        if (def is INamedResource namedDef)
        {
            _itemDef = def;
            GetNode<SpinBox>("SpinBox").Value = amount;
            GetNode<Label>("Label").Text = namedDef.Name;
            GetNode<TextureRect>("TextureRect").Texture = namedDef.Icon;
        }
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
