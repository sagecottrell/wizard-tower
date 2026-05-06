using wizardtower.events.handlers;
using wizardtower.events.ui;

namespace wizardtower.actions.ui;

public static partial class UIActions
{
    public static void BuildSelectTransport(TransportConstructionSelectingEvent @event)
    {
        var state = @event.TowerState;
        var transportDef = @event.TransportDefinition;
        var ev = TransportEvents.UI.OnTransportConstructionSelecting(new(state, transportDef));
        if (state.Wallet >= transportDef.CostToBuild && ev.IsAllowed)
            TransportEvents.UI.OnTransportConstructionSelected(new(state, transportDef) { Source = @event.Source });
    }
}
