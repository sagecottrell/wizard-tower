using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Worker;

public class WorkerEnteringTransportEvent(TowerState towerState, WorkerState workerState, TransportState transportState) : BaseEvent, ITowerEvent, IWorkerEvent, IDeniableEvent, ITransportEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public WorkerState WorkerState { get; set; } = workerState;
    public TransportState TransportState { get; set; } = transportState;
}