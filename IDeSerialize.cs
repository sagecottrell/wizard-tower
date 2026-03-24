using Godot;
using Godot.Collections;

namespace wizardtower;

public interface ISerialize
{
    Dictionary<string, Variant> Serialize();
}

public interface IDeSerialize<TSelf> : ISerialize
{
    TSelf Deserialize(Dictionary<string, Variant> dict);
}