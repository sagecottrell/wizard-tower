using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;
using wizardtower.custom_godot_resources;
using wizardtower.custom_godot_resources.helpers;

namespace wizardtower;

/// <summary>
/// IMPORTANT: Make sure you supply a default value for the Data property, otherwise the editor will not be able to create a value. The default value will not be used at runtime, so it can be an empty dictionary or contain dummy data.
/// 
/// When using this as an [Export]ed property, the keys will be stored as strings in the exported data, and converted to TKey at runtime using the LoadKey method. This allows for using Resource types as keys, which can't be directly exported as dictionary keys in Godot.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
[Tool]
public partial class NumericDict<[MustBeVariant] TKey, [MustBeVariant] TValue> : Resource,
        IDictionary<TKey, TValue>,
        IComparisonOperators<NumericDict<TKey, TValue>, NumericDict<TKey, TValue>, bool>,
        IAdditionOperators<NumericDict<TKey, TValue>, NumericDict<TKey, TValue>, NumericDict<TKey, TValue>>,
        ISubtractionOperators<NumericDict<TKey, TValue>, NumericDict<TKey, TValue>, NumericDict<TKey, TValue>>,
        IMultiplyOperators<NumericDict<TKey, TValue>, NumericDict<TKey, TValue>, NumericDict<TKey, TValue>>,
        IUnaryNegationOperators<NumericDict<TKey, TValue>, NumericDict<TKey, TValue>>,
        IAdditiveIdentity<NumericDict<TKey, TValue>, NumericDict<TKey, TValue>>
    where TKey : Resource, INamedResource
    where TValue : notnull,
        IComparisonOperators<TValue, TValue, bool>,
        IAdditionOperators<TValue, TValue, TValue>,
        ISubtractionOperators<TValue, TValue, TValue>,
        IMultiplyOperators<TValue, TValue, TValue>,
        IUnaryNegationOperators<TValue, TValue>,
        IAdditiveIdentity<TValue, TValue>
{
    private Godot.Collections.Dictionary<TKey, TValue>? runtimeData;

    [Export]
    public Godot.Collections.Dictionary<Variant, TValue> Data { get; set; } = [];

    private Godot.Collections.Dictionary<TKey, TValue> RuntimeData
    {
        get
        {
            if (runtimeData == null)
            {
                runtimeData = [];
                foreach (var (key, value) in Data)
                {
                    var v = LoadKey(key);
                    if (v is not null)
                        runtimeData[v] = value;
                }
            }
            return runtimeData;
        }
    }

    private bool _loaded = false;
    private Dictionary<string, TKey> _keyCache = [];
    protected virtual TKey? LoadKey(Variant variant)
    {
        if (!_loaded)
            _loadDefinitions();
        return _keyCache.TryGetValue(variant.ToString(), out var result) ? result : null;
    }

    private void _loadDefinitions()
    {
        if (!Engine.IsEditorHint() && _loaded)
            return;
        _loaded = true;
        var prop = typeof(TKey).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .FirstOrDefault(p => p.GetCustomAttributes(typeof(DefinitionLoaderAttribute), false).Length > 0);
        if (prop is null)
        {
            GD.PushError($"No property with DefinitionLoaderAttribute found on {typeof(TKey)}");
            return;
        }
        if (prop.GetCustomAttribute<DefinitionLoaderAttribute>(false) is not DefinitionLoaderAttribute attr)
        {
            GD.PushError($"DefinitionLoaderAttribute on {typeof(TKey)}.{prop.Name} is not of type DefinitionLoaderAttribute");
            return;
        }
        if (prop.GetValue(null) is Dictionary<string, TKey> d)
            _keyCache = d;
    }

    #region Operators

    public Dictionary<TKey, TValue> ToDictionary() => new(RuntimeData);

    public static NumericDict<TKey, TValue> AdditiveIdentity => new();

    public NumericDict<TKey, TValue> RemoveZeroes()
    {
        foreach (var (key, value) in RuntimeData)
        {
            if (value == TValue.AdditiveIdentity)
                Remove(key);
        }
        return (NumericDict<TKey, TValue>)this;
    }

    public bool TrySubtract(NumericDict<TKey, TValue> other, out NumericDict<TKey, TValue> result)
    {
        if (this >= other)
        {
            // TODO: do this in one pass instead of two
            result = this - other;
            return true;
        }
        result = default!;
        return false;
    }

    public NumericDict<TKey, TValue> Added(NumericDict<TKey, TValue> b)
    {
        foreach (var kvp in b)
        {
            if (ContainsKey(kvp.Key))
            {
                this[kvp.Key] += kvp.Value;
            }
            else
            {
                this[kvp.Key] = kvp.Value;
            }
        }
        return (NumericDict<TKey, TValue>)this;
    }

    public NumericDict<TKey, TValue> Subtracted(NumericDict<TKey, TValue> b)
    {
        foreach (var kvp in b)
        {
            if (ContainsKey(kvp.Key))
            {
                this[kvp.Key] -= kvp.Value;
            }
            else
            {
                this[kvp.Key] = -kvp.Value;
            }
        }
        return (NumericDict<TKey, TValue>)this;
    }

    public NumericDict<TKey, TValue> MultipliedComponentwise(NumericDict<TKey, TValue> b)
    {
        foreach (var kvp in b)
        {
            if (ContainsKey(kvp.Key))
            {
                this[kvp.Key] *= kvp.Value;
            }
            else
            {
                Remove(kvp.Key);
            }
        }
        return (NumericDict<TKey, TValue>)this;
    }

    public NumericDict<TKey, TValue> MultipliedByScalar(TValue scalar)
    {
        foreach (var key in Keys)
        {
            this[key] *= scalar;
        }
        return (NumericDict<TKey, TValue>)this;
    }

    public bool ContentsEqual(NumericDict<TKey, TValue>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (Count != other.Count) return false;
        foreach (var kvp in this)
        {
            if (!other.TryGetValue(kvp.Key, out var value) || !kvp.Value.Equals(value))
            {
                return false;
            }
        }
        return true;
    }

    public NumericDict<TKey, TValue> Clone()
    {
        var clone = new NumericDict<TKey, TValue>();
        foreach (var kvp in this)
        {
            clone[kvp.Key] = kvp.Value;
        }
        return clone;
    }

    public static NumericDict<TKey, TValue> operator +(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b) => a.Clone().Added(b);

    public static NumericDict<TKey, TValue> operator -(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b) => a.Clone().Subtracted(b);

    public static NumericDict<TKey, TValue> operator -(NumericDict<TKey, TValue> value) => new NumericDict<TKey, TValue>().Subtracted(value);

    public static NumericDict<TKey, TValue> operator *(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b) => a.Clone().MultipliedComponentwise(b);

    public static NumericDict<TKey, TValue> operator *(NumericDict<TKey, TValue> a, TValue scalar) => a.Clone().MultipliedByScalar(scalar);

    public static NumericDict<TKey, TValue> operator *(TValue scalar, NumericDict<TKey, TValue> a) => a.Clone().MultipliedByScalar(scalar);

    public static bool operator ==(NumericDict<TKey, TValue>? a, NumericDict<TKey, TValue>? b) => a?.ContentsEqual(b) ?? b is null;

    public static bool operator !=(NumericDict<TKey, TValue>? a, NumericDict<TKey, TValue>? b) => !(a == b);

    public static bool operator >(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b)
    {
        foreach (var kvp in a)
        {
            if (!b.TryGetValue(kvp.Key, out var value) || kvp.Value <= value)
            {
                return false;
            }
        }
        return true;
    }

    public static bool operator <(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b)
    {
        foreach (var kvp in a)
        {
            if (!b.TryGetValue(kvp.Key, out var value) || kvp.Value >= value)
            {
                return false;
            }
        }
        return true;
    }

    public static bool operator >=(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b)
    {
        foreach (var kvp in a)
        {
            if (!b.TryGetValue(kvp.Key, out var value) || kvp.Value < value)
            {
                return false;
            }
        }
        return true;
    }

    public static bool operator <=(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b)
    {
        foreach (var kvp in a)
        {
            if (!b.TryGetValue(kvp.Key, out var value) || kvp.Value > value)
            {
                return false;
            }
        }
        return true;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is null)
        {
            return false;
        }

        return obj is NumericDict<TKey, TValue> other && this == (NumericDict<TKey, TValue>)other;
    }

    public override int GetHashCode() => Data.GetHashCode();

    #endregion

    #region IDictionary<TKey, TValue> implementation

    public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)RuntimeData).Keys;

    public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)RuntimeData).Values;

    public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)RuntimeData).Count;

    public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)RuntimeData).IsReadOnly;

    public TValue this[TKey key]
    {
        get => ((IDictionary<TKey, TValue>)RuntimeData)[key]; set
        {
            RuntimeData[key] = value;
            Data[key.Name] = value;
        }
    }

    public void Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)RuntimeData).Add(key, value);
        Data.Add(key.Name, value);
    }

    public bool ContainsKey(TKey key)
    {
        return ((IDictionary<TKey, TValue>)RuntimeData).ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        Data.Remove(key.Name);
        return ((IDictionary<TKey, TValue>)RuntimeData).Remove(key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return ((IDictionary<TKey, TValue>)RuntimeData).TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)RuntimeData).Add(item);
        Data.Add(item.Key.Name, item.Value);
    }

    public void Clear()
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)RuntimeData).Clear();
        Data.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)RuntimeData).Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        Data.Remove(item.Key.Name);
        return ((ICollection<KeyValuePair<TKey, TValue>>)RuntimeData).Remove(item);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<TKey, TValue>>)RuntimeData).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)RuntimeData).GetEnumerator();
    }

    #endregion


    #region Editor

    [ExportToolButton("Edit Items")]
    public Callable EditItemsButton => Callable.From(_popupEditor);

    private void _saveItems(Godot.Collections.Dictionary data)
    {
        _editorWindow?.QueueFree();
        _editorWindow = null;
        Clear();
        foreach (var (key, value) in data)
        {
            if (key.As<TKey>() is not TKey typedKey)
            {
                GD.PushError($"Invalid key type {key.GetType()} in edited data, expected {typeof(TKey)}");
                continue;
            }
            if (value.As<TValue>() is not TValue typedValue)
            {
                GD.PushError($"Invalid value type {value.GetType()} in edited data, expected {typeof(TValue)}");
                continue;
            }
            this[typedKey] = typedValue;
        }
    }

    private Window? _editorWindow;

    private void _popupEditor()
    {
        _editorWindow = new();
        _editorWindow.CloseRequested += () => _editorWindow.QueueFree();
        EditorInterface.Singleton.PopupDialog(_editorWindow, new(50, 50, 500, 500));
        var editor = ResourceLoader.Load<PackedScene>("res://custom_godot_resources/scenes/EditorItemWindow.tscn").Instantiate<EditorItemWindow>();
        _loadDefinitions();
        editor.Setup(RuntimeData, _keyCache);
        editor.OnSave += _saveItems;
        _editorWindow.AddChild(editor);
    }

    #endregion
}

[AttributeUsage(AttributeTargets.Property)]
public class DefinitionLoaderAttribute : Attribute
{
}
