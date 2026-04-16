using Godot;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    private static Resource? _currentlyBuilding;
    public static void BuildDeselectForce(TowerState state)
    {
        switch (_currentlyBuilding)
        {
            case RoomDefinition r: GlobalSignals.RoomConstructionStopped(new(state, r)); break;
            case FloorDefinition r: GlobalSignals.FloorConstructionStopped(new(state, r)); break;
        }
        _currentlyBuilding = null;
    }
}
