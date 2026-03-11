using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace wizardtower.custom_godot_resources;

public abstract partial class GenericNumericContainer<TSelf, [MustBeVariant] TKey, [MustBeVariant] TValue> : Resource, IDictionary<TKey, TValue>
    where TKey : class, INamedResource
    where TValue : notnull, INumber<TValue>
    where TSelf : GenericNumericContainer<TSelf, TKey, TValue>, new()
{
    private Dictionary<TKey, TValue>? runtimeData;

    [Export]
    public Godot.Collections.Dictionary<Variant, TValue> Data { get; set; } = [];

    private Dictionary<TKey, TValue> RuntimeData { get 
        { 
            if (runtimeData == null)
            {
                runtimeData = [];
                foreach (var (key, value) in Data)
                {
                    runtimeData[LoadKey(key)] = value;
                }
            }
            return runtimeData;
        }
    }

    protected abstract TKey LoadKey(Variant variant);

    #region Operators

    public Dictionary<TKey, TValue> ToDictionary() => new(RuntimeData);

    public TSelf RemoveZeroes()
    {
        foreach (var (key, value) in RuntimeData)
        {
            if (value == TValue.Zero)
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

    public TSelf Added(TSelf b)
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

    public TSelf Subtracted(TSelf b)
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

    public TSelf MultipliedComponentwise(TSelf b)
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

    public bool ContentsEqual(TSelf? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (Count != other.Count) return false;
        foreach (var kvp in this)
        {
            if (!other.TryGetValue(kvp.Key, out TValue value) || !kvp.Value.Equals(value))
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

    public static TSelf operator *(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b) => a.Clone().MultipliedComponentwise(b);

    public static TSelf operator *(GenericNumericContainer<TSelf, TKey, TValue> a, TValue scalar) => a.Clone().MultipliedByScalar(scalar);

    public static TSelf operator *(TValue scalar, GenericNumericContainer<TSelf, TKey, TValue> a) => a.Clone().MultipliedByScalar(scalar);

    public static bool operator ==(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b) => a?.ContentsEqual(b) ?? b is null;

    public static bool operator !=(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b) => !(a == b);

    public static bool operator >(GenericNumericContainer<TSelf, TKey, TValue> a, TSelf b)
    {
        foreach (var kvp in a)
        {
            if (!b.TryGetValue(kvp.Key, out TValue value) || kvp.Value <= value)
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
            if (!b.TryGetValue(kvp.Key, out TValue value) || kvp.Value >= value)
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
            if (!b.TryGetValue(kvp.Key, out TValue value) || kvp.Value < value)
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
            if (!b.TryGetValue(kvp.Key, out TValue value) || kvp.Value > value)
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
            Data[key.ItemName] = value;
        }
    }

    public void Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)RuntimeData).Add(key, value);
        Data.Add(key.ItemName, value);
    }

    public bool ContainsKey(TKey key)
    {
        return ((IDictionary<TKey, TValue>)RuntimeData).ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        Data.Remove(key.ItemName);
        return ((IDictionary<TKey, TValue>)RuntimeData).Remove(key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return ((IDictionary<TKey, TValue>)RuntimeData).TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)RuntimeData).Add(item);
        Data.Add(item.Key.ItemName, item.Value);
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
        Data.Remove(item.Key.ItemName);
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

}
