using Godot;
using System.Linq;
using System.Reflection;

namespace wizardtower.resource_types.scenes;

[Tool]
[GlobalClass]
public partial class EditorItemSelector : Control
{
    private Resource? _itemDef;

    [Signal]
    public delegate void OnAmountChangedEventHandler(Resource def, Variant value);

    public void SetItemDefinition<[MustBeVariant] T>(Resource def, Variant amount)
        where T : new()
    {
        _itemDef = def;
        var type = typeof(T);
        if (def is INamedResource namedDef)
        {
            if (type.IsAssignableTo(typeof(long)) || type.IsAssignableTo(typeof(uint)))
            {
                GetNode<SpinBox>("SpinBox").Visible = true;
                GetNode<SpinBox>("SpinBox").Value = amount.AsInt64();
            }
            else if (type.GetProperties().FirstOrDefault(x => x.GetCustomAttribute<ExportToolButtonAttribute>() != null) is PropertyInfo prop)
            {
                var btn = GetNode<Button>("Button");
                btn.Visible = true;
                btn.Pressed += () =>
                {
                    if (amount.Obj is null)
                        amount = Variant.From(new T());
                    if (prop.GetValue(amount.Obj) is Callable cb)
                        cb.Call();
                    if (amount.AsGodotObject() is GodotObject obj && obj.HasSignal("OnChanged"))
                    {
                        obj.Connect("OnChanged", Callable.From(() => EmitSignalOnAmountChanged(def, amount)));
                    }
                    EmitSignalOnAmountChanged(def, amount);
                };
            }

            GetNode<Label>("Label").Text = namedDef.Name;
            GetNode<TextureRect>("TextureRect").Texture = namedDef.Icon ?? ResourceLoader.Load(namedDef.GetType().GetCustomAttribute<IconAttribute>()?.Path) as Texture2D;
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
