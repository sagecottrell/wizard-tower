using wizardtower.state;

namespace wizardtower.events.interfaces;

public interface IWorkerEvent
{
    WorkerState WorkerState { get; set; }
}
