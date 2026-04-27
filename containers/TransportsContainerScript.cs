using Godot;
using System.Collections.Generic;
using System.Linq;
using wizardtower.actions.ui;
using wizardtower.events;
using wizardtower.events.ui;
using wizardtower.state;

namespace wizardtower.containers;

public partial class TransportsContainerScript(TowerScript tower) : Node3D()
{
    public TowerScript Tower { get; } = tower;
    public TowerState State { get; } = tower.State;

    private readonly Dictionary<TransportState, TransportScript> _nodes = [];

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
        GlobalSignals.Singleton.OnTransportDestroyed += _onTransportDestroyed;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnTransportConstructing -= _onTransportConstructing;
        GlobalSignals.Singleton.OnTransportConstructionStopping -= _onTransportConstructionStopping;
        GlobalSignals.Singleton.OnTransportConstructed -= _onTransportConstructed;
        GlobalSignals.Singleton.OnTransportDestroyed -= _onTransportDestroyed;
    }

    private void _onTransportConstructed(TransportConstructedEvent @event)
    {
        SetupTransportationDisplay(@event.Transport);
    }

    private void _onTransportConstructing(TransportConstructingEvent @event)
    {
        if (@event.Transport.Definition.CostToBuild > State.Wallet)
        {
            this.Log("Not enough money to build this transport.");
            @event.IsAllowed = false;
            return;
        }
        // enough money means it is allowed to build
    }

    private void _onTransportConstructionStopping(TransportConstructionStoppingEvent @event)
    {
        if (@event.TransportDefinition.CostToBuild <= State.Wallet)
            @event.IsAllowed = false;
    }

    public void SetupTransportationDisplay(TransportState newTransport)
    {
        var transport = new TransportScript()
        {
            State = newTransport,
        };
        _nodes[newTransport] = transport;
        AddChild(transport);
    }

    private void _onTransportDestroyed(TransportDestroyedEvent @event)
    {
        if (_nodes.Remove(@event.Transport, out var node))
            node.QueueFree();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton ms && ms.IsActionPressed(InputMapConstants.LeftClick) && GetViewport().GetCamera3D() is Camera3D camera)
        {
            var dir = camera.ProjectRayOrigin(ms.Position);
            var x = (int)(dir.X + 0.5f);
            var y = (int)dir.Y;
            if (Tower.State.TransportsOnFloor(y).FirstOrDefault(r => x >= r.HorizontalPosition && x < r.HorizontalPosition + r.Definition.Width) is TransportState transport)
            {
                UIActions.SelectTransport(new(Tower.State, transport) { Input = @event });
            }
        }
    }
}
