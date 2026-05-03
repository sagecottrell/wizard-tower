using Godot;
using System.Numerics;
using wizardtower.resource_types;
using wizardtower.resource_types.scenes;

namespace wizardtower.addons.project_plugins;

public partial class NumericDictInspector : EditorInspectorPlugin
{
    public override bool _CanHandle(GodotObject @object)
    {
        return @object is not INumericDict;
    }

    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        var item = @object.Get(name);
        if (item.Obj is INumericDict dict && SceneLoader.TryLoadScene<EditorItemWindow>(out var editor))
        {
            GetType().GetMethod(nameof(_createEditor), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.MakeGenericMethod(dict.KeyType, dict.ValueType)
                .Invoke(this, [editor, dict, name]);
        }
        return false;
    }

    private void _createEditor<[MustBeVariant] TKey, [MustBeVariant] TValue>(EditorItemWindow editor, NumericDict<TKey, TValue> dict, string name)
        where TKey : Resource, INamedResource, new()
        where TValue : notnull,
            IComparisonOperators<TValue, TValue, bool>,
            IAdditionOperators<TValue, TValue, TValue>,
            ISubtractionOperators<TValue, TValue, TValue>,
            IMultiplyOperators<TValue, TValue, TValue>,
            IUnaryNegationOperators<TValue, TValue>,
            IAdditiveIdentity<TValue, TValue>,
            new()
    {
        editor.Setup(dict.ToGodotDictionary(), LoadDefs.LoadAll<TKey>());
        var editorProperty = new EditorProperty();
        editorProperty.AddChild(editor);
        editor.OnChange += (def, value) =>
        {
            if (def.As<TKey>() is not TKey typedKey)
            {
                GD.PushError($"Invalid key type {def.GetType()} in edited data, expected {typeof(TKey)}");
                return;
            }
            if (value.As<TValue>() is not TValue typedValue)
            {
                GD.PushError($"Invalid value type {value.GetType()} in edited data, expected {typeof(TValue)}");
                return;
            }
            dict[typedKey] = typedValue;
            editorProperty.EmitChanged(name, dict);
        };
        editor.OnRemove += (def) =>
        {
            if (def.As<TKey>() is not TKey typedKey)
            {
                GD.PushError($"Invalid key type {def.GetType()} in edited data, expected {typeof(TKey)}");
                return;
            }
            dict.Remove(typedKey);
            editorProperty.EmitChanged(name, dict);
        };
        editorProperty.Ready += () =>
        {
            editorProperty.Label = "";
            editorProperty.ReadOnly = true;
            editorProperty.NameSplitRatio = 0;
        };
        AddPropertyEditor(name, editorProperty, true);
    }

}
