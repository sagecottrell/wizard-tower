using System.Collections.Generic;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room.ui;


/// <summary>
/// <para>Used to get a list of next possible destinations for the delivery path.</para>
/// <para>The intention is that rooms and transports will listen for this event, look at <see cref="Path"/> and <see cref="ItemDefinitions"/>
/// and potentially add items to <see cref="NextValidPositions"/>.</para>
/// </summary>
public class RoomDeliveryPlanningQueryEvent(TowerState towerState, RoomState startingRoom, List<ItemDefinition> itemDefinitions, RoomStateWorkerPath path) : BaseEvent, ITowerEvent
{
    public TowerState TowerState { get; } = towerState;

    public RoomState StartingRoom { get; } = startingRoom;

    public List<ItemDefinition> ItemDefinitions { get; } = itemDefinitions;

    public RoomStateWorkerPath Path { get; } = path;

    public HashSet<ValidPosition> NextValidPositions { get; } = [];

    public class ValidPosition
    {
        public int Elevation { get; set; }
        public int Position { get; set; }
        public PosKind? Kind { get; set; }
        public uint Width { get; set; } = 1;
        public uint Height { get; set; } = 1;

        public Godot.Vector3 ToVector3() => new(Position, Elevation, 0);

        public abstract record class PosKind;

        public record class TransportKind(TransportState State) : PosKind;
        public record class RoomKind(RoomState State) : PosKind;
    }
}
