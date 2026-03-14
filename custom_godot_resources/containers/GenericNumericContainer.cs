using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace wizardtower.custom_godot_resources.containers;

[Tool]
public abstract partial class GenericNumericContainer<TSelf, [MustBeVariant] TKey, [MustBeVariant] TValue> : Resource,
        IDictionary<TKey, TValue>,
        IComparisonOperators<GenericNumericContainer<TSelf, TKey, TValue>, TSelf, bool>,
        IAdditionOperators<GenericNumericContainer<TSelf, TKey, TValue>, TSelf, TSelf>,
        ISubtractionOperators<GenericNumericContainer<TSelf, TKey, TValue>, TSelf, TSelf>,
        IMultiplyOperators<GenericNumericContainer<TSelf, TKey, TValue>, TSelf, TSelf>,
        IUnaryNegationOperators<GenericNumericContainer<TSelf, TKey, TValue>, TSelf>,
        IAdditiveIdentity<GenericNumericContainer<TSelf, TKey, TValue>, TSelf>
    where TKey : Resource, INamedResource
    where TValue : notnull,
        IComparisonOperators<TValue, TValue, bool>,
        IAdditionOperators<TValue, TValue, TValue>,
        ISubtractionOperators<TValue, TValue, TValue>,
        IMultiplyOperators<TValue, TValue, TValue>,
        IUnaryNegationOperators<TValue, TValue>,
        IAdditiveIdentity<TValue, TValue>
    where TSelf : GenericNumericContainer<TSelf, TKey, TValue>, new()
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

    public static TSelf AdditiveIdentity => new();

    public TSelf RemoveZeroes()
    {
        foreach (var (key, value) in RuntimeData)
        {
            if (value == TValue.AdditiveIdentity)
                Remove(key);
        }
        return (TSelf)this;
    }

    public bool TrySubtract(TSelf other, out TSelf result)
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

    public TSelf Added(GenericNumericContainer<TSelf, TKey, TValue> b)
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
        return (TSelf)this;
    }

    public TSelf Subtracted(GenericNumericContainer<TSelf, TKey, TValue> b)
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
        return (TSelf)this;
    }

    public TSelf MultipliedComponentwise(GenericNumericContainer<TSelf, TKey, TValue> b)
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
        return (TSelf)this;
    }

    public TSelf MultipliedByScalar(TValue scalar)
    {
        foreach (var key in Keys)
        {
            this[key] *= scalar;
        }
        return (TSelf)this;
    }

    public bool ContentsEqual(GenericNumericContainer<TSelf, TKey, TValue>? other)
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

    public TSelf Clone()
    {
        var clone = new TSelf();
        foreach (var kvp in this)
        {
            clone[kvp.Key] = kvp.Value;
        }
        return clone;
    }

    public static TSelf operator +(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b) => a.Clone().Added(b);

    public static TSelf operator -(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b) => a.Clone().Subtracted(b);

    public static TSelf operator -(GenericNumericContainer<TSelf, TKey, TValue> value) => new TSelf().Subtracted(value);

    public static TSelf operator *(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b) => a.Clone().MultipliedComponentwise(b);

    public static TSelf operator *(GenericNumericContainer<TSelf, TKey, TValue> a, TValue scalar) => a.Clone().MultipliedByScalar(scalar);

    public static TSelf operator *(TValue scalar, GenericNumericContainer<TSelf, TKey, TValue> a) => a.Clone().MultipliedByScalar(scalar);

    public static bool operator ==(GenericNumericContainer<TSelf, TKey, TValue>? a, TSelf? b) => a?.ContentsEqual(b) ?? b is null;

    public static bool operator !=(GenericNumericContainer<TSelf, TKey, TValue>? a, TSelf? b) => !(a == b);

    public static bool operator >(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b)
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

    public static bool operator <(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b)
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

    public static bool operator >=(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b)
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

    public static bool operator <=(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b)
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

        return obj is GenericNumericContainer<TSelf, TKey, TValue> other && this == (TSelf)other;
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
        var editor = ResourceLoader.Load<PackedScene>("res://custom_godot_resources/scenes/EditorItemWindow.tscn").Instantiate<helpers.EditorItemWindow>();
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
