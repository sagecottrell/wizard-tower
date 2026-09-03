using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

/// <summary>
/// When the user selects a room to construct, the game needs to calculate where the room can be placed and show valid placements.
/// This event is used to calculate valid placements. by allowing anything to set IsAllowed to false on the pre-event, they can block placement.
/// </summary>
public class RoomConstructionSelectorShowedEvent(TowerState tower, RoomDefinition roomDefinition, int elevation, int position) : BaseEvent, ITowerEvent
{
    public Rect2I Collision = new(position, elevation, (int)roomDefinition.Width, (int)roomDefinition.Height);

    public TowerState TowerState { get; set; } = tower;
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
    public int Elevation { get; set; } = elevation;
    public int Position { get; set; } = position;
}