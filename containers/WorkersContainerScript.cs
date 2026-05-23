using Godot;
using System.Collections.Generic;
using wizardtower.events.handlers;
using wizardtower.events.interfaces;
using wizardtower.events.Worker;
using wizardtower.state;

namespace wizardtower.containers;

public partial class WorkersContainerScript(TowerScript towerScript) : Node3D
{
    public TowerScript TowerScript { get; } = towerScript;

    private readonly Dictionary<WorkerState, WorkerScript> _workers = [];

    public override void _Ready()
    {
        Name = nameof(WorkersContainerScript);
    }

    public override void _EnterTree()
    {
        WorkerEvents.Dispatched += _onWorkerDispatched;
        WorkerEvents.EnteredTransport += _onWorkerDespawn;
    }

    public override void _ExitTree()
    {
        WorkerEvents.Dispatched -= _onWorkerDispatched;
        WorkerEvents.EnteredTransport -= _onWorkerDespawn;
    }

    private void _onWorkerDespawn(IWorkerEvent ev)
    {
        if (_workers.TryGetValue(ev.WorkerState, out var script))
        {
            script.Visible = false;
            if (script.GetParent() == this)
                RemoveChild(script);
        }
    }

    private void _onWorkerDispatched(WorkerDispatchedEvent e)
    {
        var s = new WorkerScript();
        _workers[e.WorkerState] = s;
        AddChild(s);
    }
}
