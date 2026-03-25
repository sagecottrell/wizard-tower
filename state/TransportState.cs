using Godot;
using System;
using wizardtower.resource_types;

namespace wizardtower.state;

public partial class TransportState : Resource, ICopy<TransportState>, IDeSerialize<TransportState>
{
    [Export]
    public TransportDefinition? Definition { get; set; }

    public TransportState Copy() => new()
    {
        Definition = Definition,
    };

    public Godot.Collections.Dictionary<string, Variant> Serialize() => [];
    public TransportState Deserialize(Godot.Collections.Dictionary<string, Variant> dict) => this;
}
