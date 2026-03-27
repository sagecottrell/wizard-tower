using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;

namespace wizardtower;

public interface ISerialize
{
    Dictionary<string, Variant> Serialize() {
        var props = GetType().GetProperties().Where(x => x.GetCustomAttribute<DeSerializeAttribute>() != null).ToList();
        var returnValues = new Dictionary<string, Variant>();
        foreach (var prop in props)
        {
            var value = prop.GetValue(this);
            if (value is ISerialize s)
                returnValues.Add(prop.Name, s.Serialize());
            else
                returnValues.Add(prop.Name, Variant.From(value));
        }
        return returnValues;
    }
}

public interface IDeSerialize
{
    object DeserializeObject(Dictionary<string, Variant> dict, bool ignoreExtraProps = false)
    {
        var t = GetType();
        foreach (var (key, value) in dict)
        {
            if (t.GetProperty(key) is not PropertyInfo propertyInfo)
            {
                if (!ignoreExtraProps)
                    throw new InvalidOperationException($"\"{key}\" is not a property on {t.FullName}");
                continue;
            }

            if (propertyInfo.PropertyType.IsAssignableTo(typeof(IDeSerialize)) && Activator.CreateInstance(propertyInfo.PropertyType) is IDeSerialize obj)
            {
                propertyInfo.SetValue(this, obj.DeserializeObject(dict, ignoreExtraProps));
            }
            else if (value.AsSaveFormatDict() is Dictionary<string, Variant> vDict)
            {
                // TODO
            }
            else
            {
                propertyInfo.SetValue(this, value);
            }
        }
        return this;
    }
}

public interface IDeSerialize<TSelf> : IDeSerialize, ISerialize
{
    TSelf Deserialize(Dictionary<string, Variant> dict, bool ignoreExtraProps = false) => (TSelf)DeserializeObject(dict, ignoreExtraProps);
}


[AttributeUsage(AttributeTargets.Property)]
public class DeSerializeAttribute : Attribute
{
}