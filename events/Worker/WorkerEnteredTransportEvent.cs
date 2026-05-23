using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.Worker;

public class WorkerEnteredTransportEvent(TowerState towerState, WorkerState workerState, TransportState transportState) : BaseEvent, ITowerEvent, IWorkerEvent, ITransportEvent
{
    public TowerState TowerState { get; } = towerState;
    public WorkerState WorkerState { get; set; } = workerState;
    public TransportState TransportState { get; set; } = transportState;
}
