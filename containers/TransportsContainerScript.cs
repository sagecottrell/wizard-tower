using Godot;
using System;
using wizardtower.events;
using wizardtower.events.ui;
using wizardtower.state;

namespace wizardtower.containers;

public partial class TransportsContainerScript(TowerScript tower) : Node3D()
{
    public TowerScript Tower { get; } = tower;
    public TowerState State { get; } = tower.State;

    public override void _Ready()
    {
        foreach (var transport in State.Transports.Values)
            SetupTransportationDisplay(transport);
    }
    public override void _EnterTree()
    {
        GlobalSignals.Singleton.OnTransportConstructing += _onTransportConstructing;
        GlobalSignals.Singleton.OnTransportConstructionStopping += _onTransportConstructionStopping;
        GlobalSignals.Singleton.OnTransportConstructed += _onTransportConstructed;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnTransportConstructing -= _onTransportConstructing;
        GlobalSignals.Singleton.OnTransportConstructionStopping -= _onTransportConstructionStopping;
        GlobalSignals.Singleton.OnTransportConstructed -= _onTransportConstructed;
    }

    private void _onTransportConstructed(TransportConstructedEvent @event)
    {
        SetupTransportationDisplay(@event.Transport);
    }

    private void _onTransportConstructing(TransportConstructingEvent @event)
    {
    }

    private void _onTransportConstructionStopping(TransportConstructionStoppingEvent @event)
    {
    }

    public void SetupTransportationDisplay(TransportState newTransport)
    {
        var transport = new TransportScript()
        {
            State = newTransport,
        };
        AddChild(transport);

        //for (var i = 0; i < newTransport.Height; i++)
        //{
        //    if (Floors.TryGetValue(newTransport.Elevation + i, out var fs))
        //    {
        //        fs.SetPositionVisible(newTransport.HorizontalPosition, newTransport.Definition.Width, false);
        //    }
        //}
    }
}
