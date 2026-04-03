using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using wizardtower.resource_types;
using wizardtower.resource_types.scenes;

namespace wizardtower;

public interface IReadonlyNumericDict { }

public interface IReadonlyNumericDict<TSelf, TKey, TValue> :
        IComparisonOperators<TSelf, TSelf, bool>,
        IAdditionOperators<TSelf, TSelf, TSelf>,
        ISubtractionOperators<TSelf, TSelf, TSelf>,
        IMultiplyOperators<TSelf, TSelf, TSelf>,
        IUnaryNegationOperators<TSelf, TSelf>,
        IAdditiveIdentity<TSelf, TSelf>,
        IReadonlyNumericDict
    where TSelf : IReadonlyNumericDict<TSelf, TKey, TValue>
{ }

/// <summary>
/// IMPORTANT: Make sure you supply a default value for the Data property, otherwise the editor will not be able to create a value. The default value will not be used at runtime, so it can be an empty dictionary or contain dummy data.
/// 
/// When using this as an [Export]ed property, the keys will be stored as strings in the exported data, and converted to TKey at runtime using the LoadKey method. This allows for using Resource types as keys, which can't be directly exported as dictionary keys in Godot.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
[Tool]
public sealed partial class NumericDict<[MustBeVariant] TKey, [MustBeVariant] TValue> : Resource,
        IDictionary<TKey, TValue>, 
        IReadonlyNumericDict<NumericDict<TKey, TValue>, TKey, TValue>,
        IToBBCode,
        ICopy<NumericDict<TKey, TValue>>,
        IDeSerialize<NumericDict<TKey, TValue>>
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

    [Signal]
    public delegate void OnChangedEventHandler();

    [Export]
    public Godot.Collections.Dictionary<string, TValue> Data { 
        get {
            data ??= [];
            return data;
        } 
        set => data = value; 
    }

    private static readonly Dictionary<string, TKey> _keyCache = [];
    private static TKey _loadKey(string variant)
    {
        if (_keyCache.TryGetValue(variant, out var cached))
            return cached;
        var loaded = ResourceLoader.Load<TKey>(variant);
        if (loaded is null)
        {
            GD.PushError($"Failed to load key of type {typeof(TKey)} with path {variant}");
            loaded = new();
        }
        _keyCache[variant] = loaded;
        return loaded;
    }

    #region Operators

    public Godot.Collections.Dictionary<TKey, TValue> ToGodotDictionary() => new(Data.ToDictionary(kvp => _loadKey(kvp.Key), kvp => kvp.Value));

    private static NumericDict<TKey, TValue>? _additiveIdentity;
    private Godot.Collections.Dictionary<string, TValue>? data;

    public static NumericDict<TKey, TValue> AdditiveIdentity { 
        get {
            if (_additiveIdentity is null || _additiveIdentity.Count != 0)
                // just in case someone modifies the additive identity, we want to be able to recreate it
                _additiveIdentity = [];
            return _additiveIdentity;
        }
    }

    /// <summary>
    /// Removes any entries with a value of zero (or whatever the additive identity is for TValue) from the dictionary. 
    /// This is useful for keeping the dictionary clean after performing operations that may result in zero values, such as subtraction. 
    /// Note that this modifies the current instance, it does not return a new instance. It returns this instance for chaining purposes.
    /// </summary>
    /// <returns></returns>
    public NumericDict<TKey, TValue> RemoveZeroes()
    {
        foreach (var (key, value) in Data)
        {
            if (value == TValue.AdditiveIdentity)
                Data.Remove(key);
        }
        EmitSignalOnChanged();
        return (NumericDict<TKey, TValue>)this;
    }

    /// <summary>
    /// Attempts subtraction, returning false if the result would contain any negative values. Note that this does not modify the current instance, it just checks if the subtraction is valid and returns the result if it is.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="result"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Mutates and adds the value
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public NumericDict<TKey, TValue> Added(NumericDict<TKey, TValue> b)
    {
        foreach (var (key, value) in b)
        {
            if (ContainsKey(key))
            {
                _set(key, this[key] + value);
            }
            else
            {
                _set(key, value);
            }
        }
        EmitSignalOnChanged();
        return (NumericDict<TKey, TValue>)this;
    }

    /// <summary>
    /// Mutates and subtracts the value 
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public NumericDict<TKey, TValue> Subtracted(NumericDict<TKey, TValue> b)
    {
        foreach (var (key, value) in b)
        {
            if (ContainsKey(key))
            {
                _set(key, this[key] - value);
            }
            else
            {
                _set(key, -value);
            }
        }
        EmitSignalOnChanged();
        return (NumericDict<TKey, TValue>)this;
    }

    /// <summary>
    /// Mutates and multiplies the value componentwise. Note that this is not a standard vector operation, and it may not have a clear mathematical meaning in all contexts.
    /// Symmetry is not guaranteed, as if one dictionary has a key that the other does not, the result will be different depending on which dictionary is the caller.
    /// It can be useful for certain applications, such as applying a multiplier to a set of values, but it should be used with caution and clearly documented to avoid confusion.
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public NumericDict<TKey, TValue> MultipliedComponentwise(NumericDict<TKey, TValue> b)
    {
        foreach (var (key, value) in b)
        {
            if (ContainsKey(key))
            {
                _set(key, this[key] * value);
            }
            else
            {
                _remove(key);
            }
        }
        EmitSignalOnChanged();
        return (NumericDict<TKey, TValue>)this;
    }

    /// <summary>
    /// Mutates and multiplies the value by a scalar. This is a standard vector operation, and it should have a clear mathematical meaning in most contexts. It multiplies every value in the dictionary by the given scalar, regardless of the keys.
    /// </summary>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public NumericDict<TKey, TValue> MultipliedByScalar(TValue scalar)
    {
        foreach (var key in Keys)
        {
            _set(key, this[key] * scalar);
        }
        EmitSignalOnChanged();
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
            clone._set(kvp.Key, kvp.Value);
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
        // loop over B because A must have every key in B to be greater, but A can have extra keys that aren't in B and still be greater
        foreach (var (key, value) in b)
        {
            if (a.GetOrDefault(key, TValue.AdditiveIdentity) <= value)
                return false;
        }
        return true;
    }

    public static bool operator <(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b) => b > a;

    public static bool operator >=(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b)
    {
        // loop over B because A must have every key in B to be greater, but A can have extra keys that aren't in B and still be greater
        foreach (var (key, value) in b)
        {
            if (a.GetOrDefault(key, TValue.AdditiveIdentity) < value)
                return false;
        }
        return true;
    }

    public static bool operator <=(NumericDict<TKey, TValue> a, NumericDict<TKey, TValue> b) => b >= a;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj == null)
            return false;
        if (obj.Equals(0L) || obj.Equals(0))
        {
            return Data.Values.All(x => x.Equals(0));
        }

        return obj is NumericDict<TKey, TValue> other && this == other;
    }

    public override int GetHashCode() => Data.GetHashCode();

    #endregion

    public TValue GetOrDefault(TKey key, TValue defaultValue = default!) => TryGetValue(key, out var value) ? value : defaultValue;

    #region IDictionary<TKey, TValue> implementation

    public ICollection<TKey> Keys => [..Data.Keys.Select(_loadKey)];

    public ICollection<TValue> Values => Data.Values;

    public int Count => Data.Count;

    public bool IsReadOnly => ((ICollection<KeyValuePair<string, TValue>>)Data).IsReadOnly;

    private void _set(TKey key, TValue value)
    {
        Data[key.ResourcePath] = value;
    }

    private bool _remove(TKey key)
    {
        return Data.Remove(key.ResourcePath);
    }

    public TValue this[TKey key]
    {
        get => Data[key.ResourcePath]; 
        set
        {
            _set(key, value);
            EmitSignalOnChanged();
        }
    }

    public void Add(TKey key, TValue value)
    {
        Data.Add(key.ResourcePath, value);
        EmitSignalOnChanged();
    }

    public bool ContainsKey(TKey key)
    {
        return Data.ContainsKey(key.ResourcePath);
    }

    public bool Remove(TKey key)
    {
        if (_remove(key))
        {
            EmitSignalOnChanged();
            return true;
        }
        return false;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return Data.TryGetValue(key.ResourcePath, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Data.Add(item.Key.ResourcePath, item.Value);
        EmitSignalOnChanged();
    }

    public void Clear()
    {
        Data.Clear();
        EmitSignalOnChanged();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return Data.TryGetValue(item.Key.ResourcePath, out var value) && value == item.Value;
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return Data
            .Select(kvp => {
                var key = _loadKey(kvp.Key);
                if (key is null)
                    GD.PushError($"Failed to load key of type {typeof(TKey)} with path {kvp.Key}");
                return key;
            })
            .Where(key => key is not null)
            .Select(key => new KeyValuePair<TKey, TValue>(key!, Data[key!.ResourcePath]))
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Data.GetEnumerator();
    }

    #endregion


    #region Editor

    [ExportToolButton("Edit Items")]
    public Callable EditItemsButton => Callable.From(_popupEditor);

    private void _saveItems(Godot.Collections.Dictionary data)
    {
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

    private void _popupEditor()
    {
        if (SceneLoader.TryLoadScene<EditorItemWindow>(out var editor))
        {
            editor.Setup(ToGodotDictionary(), LoadDefs.LoadAll<TKey>());
            var id = EditorWindowHelper.PopupEditor(editor);
            editor.OnSave += (data) =>
            {
                EditorWindowHelper.Close(id);
                _saveItems(data);
            };
        }
    }

    #endregion

    #region IToBBCode

    public string ToStringBBCode() => typeof(TValue).IsAssignableTo(typeof(Resource)) ? $"{{{string.Join(
        ",",
        Data.Select(x => $"[img height=24]{NumericDict<TKey, TValue>._loadKey(x.Key)?.IconPathOrName}[/img]:{(x.Value is IToBBCode bbcode ? bbcode.ToStringBBCode() : x.Value)}")
        )}}}" : string.Join(" ", Data.Select(x => $"{(x.Value is IToBBCode bbcode ? bbcode.ToStringBBCode() : x.Value)}[img height=24]{NumericDict<TKey, TValue>._loadKey(x.Key)?.IconPathOrName}[/img]"));

    #endregion

    #region IDeSerialize

    public Godot.Collections.Dictionary<string, Variant> Serialize() => new(Data.ToDictionary(
        kvp => kvp.Key, 
        kvp => kvp.Value is ISerialize serializable ? Variant.From(serializable.Serialize()) : Variant.From(kvp.Value)
        ));

    public NumericDict<TKey, TValue> Deserialize(Godot.Collections.Dictionary<string, Variant> dict)
    {
        Data = new(dict
            .Where(kvp => kvp.Key is not null)
            .ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value is IDeSerialize<TValue> ds ? ds.Deserialize(kvp.Value.AsGodotDictionary<string, Variant>()) : kvp.Value.As<TValue>()
        ));
        return this;
    }

    #endregion

    #region ICopy

    public NumericDict<TKey, TValue> Copy() => new() { 
        Data = new(Data.ToDictionary()), 
    };

    #endregion

    public override string ToString() => $"{{{string.Join(
        ",",
        Data.Select(x => $"{_loadKey(x.Key).Name}:{x.Value}")
        )}}}";

    public string ToStringAsCost() => typeof(TValue).IsAssignableTo(typeof(Resource)) ? $"{{{string.Join(
        " + ",
        Data.Select(x => $"[img height=24]{NumericDict<TKey, TValue>._loadKey(x.Key)?.IconPathOrName}[/img]:{(x.Value is IToBBCode bbcode ? bbcode.ToStringBBCode() : x.Value)}")
        )}}}" : string.Join(" + ", Data.Select(x => $"{(x.Value is IToBBCode bbcode ? bbcode.ToStringBBCode() : x.Value)}[img height=24]{NumericDict<TKey, TValue>._loadKey(x.Key)?.IconPathOrName}[/img]"));
}
