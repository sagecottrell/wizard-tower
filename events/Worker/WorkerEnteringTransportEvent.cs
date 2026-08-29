/**
Generated from ./events/Worker/WorkerEnteredTransportEvent.cs
**/

using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Worker;

public class WorkerEnteringTransportEvent(TowerState towerState, WorkerState workerState, TransportState transportState) : BaseEvent, IDeniableEvent, ITowerEvent, IWorkerEvent, ITransportEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; set; } = towerState;
    public WorkerState WorkerState { get; set; } = workerState;
    public TransportState TransportState { get; set; } = transportState;
}


public static class WorkerEnteredTransportEventExtensions {
    public static WorkerEnteredTransportEvent Into(this WorkerEnteringTransportEvent old) {
        return new(towerState: old.TowerState, workerState: old.WorkerState, transportState: old.TransportState) { Source = old, Input = old.Input, };
    }
}