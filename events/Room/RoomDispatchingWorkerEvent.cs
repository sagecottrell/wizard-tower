using wizardtower.events.interfaces;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.events.Room;

public class RoomDispatchingWorkerEvent(TowerState towerState, RoomState roomState, RoomState targetRoom, ItemDefinition item, uint amount, WorkerDefinition workerDefinition) : BaseEvent, IDeniableEvent, ITowerEvent, IRoomEvent
{
    public bool IsAllowed { get; set; } = true;
    public TowerState TowerState { get; } = towerState;
    public RoomState RoomState { get; set; } = roomState;
    public RoomState TargetRoom { get; set; } = targetRoom;
    public ItemDefinition Item { get; set; } = item;
    public uint Amount { get; set; } = amount;
    public WorkerDefinition WorkerDefinition { get; } = workerDefinition;
}
