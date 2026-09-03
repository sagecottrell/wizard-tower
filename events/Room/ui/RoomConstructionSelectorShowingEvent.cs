/**
Generated from ./events/Room/ui/RoomConstructionSelectorShowedEvent.cs
**/

using Godot;
using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room.ui;

/// <summary>
/// When the user selects a room to construct, the game needs to calculate where the room can be placed and show valid placements.
/// This event is used to calculate valid placements. by allowing anything to set IsAllowed to false on the pre-event, they can block placement.
/// </summary>
public class RoomConstructionSelectorShowingEvent(TowerState tower, RoomDefinition roomDefinition, int elevation, int position) : BaseEvent, IDeniableEvent, ITowerEvent
{
    public bool IsAllowed { get; set; } = true;
    public Rect2I Collision = new(position, elevation, (int)roomDefinition.Width, (int)roomDefinition.Height);

    public TowerState TowerState { get; set; } = tower;
    public RoomDefinition RoomDefinition { get; set; } = roomDefinition;
    public int Elevation { get; set; } = elevation;
    public int Position { get; set; } = position;
}

public static class RoomConstructionSelectorShowedEventExtensions {
    public static RoomConstructionSelectorShowedEvent Into(this RoomConstructionSelectorShowingEvent old) {
        return new(tower: old.TowerState, roomDefinition: old.RoomDefinition, elevation: old.Elevation, position: old.Position) { Source = old, };
    }

    public static RoomConstructionSelectorShowingEvent RoomConstructionSelectorShowingEvent(this IEvent ev, TowerState tower, RoomDefinition roomDefinition, int elevation, int position)
    {
        return new(tower, roomDefinition, elevation, position) { Source = ev };
    }
}