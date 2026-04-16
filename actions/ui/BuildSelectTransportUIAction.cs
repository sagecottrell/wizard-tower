using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void BuildSelectTransport(TransportConstructionSelectingEvent @event)
    {
        if (_currentlyBuilding == @event.TransportDefinition)
            return;
        var state = @event.TowerState;
        var transportDef = @event.TransportDefinition;
        BuildDeselectForce(@event.TowerState);
        _currentlyBuilding = @event.TransportDefinition;
        var ev = GlobalSignals.TransportConstructionSelecting(new(state, transportDef));
        if (state.Wallet >= transportDef.CostToBuild && ev.IsAllowed)
            GlobalSignals.TransportConstructionSelected(new(state, transportDef));
    }
}
