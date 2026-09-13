using System.Linq;
using Godot;
using wizardtower.state;

namespace wizardtower.containers;

public partial class WorkerScript(TowerState towerState, WorkerState worker) : Node3D
{
    public TowerState TowerState { get; } = towerState;
    public WorkerState Worker { get; } = worker;

    private RoomStateWorkerPath? path = towerState.Rooms.TryGetValue(worker.SourceRoomId, out var source) && towerState.Rooms.TryGetValue(worker.DestinationRoomId, out var destination)
        ? source.GetPathTo(destination)
        : null;

    private RoomState? walkingToRoom;
    private TransportState? walkingToTransport;

    private Node? node;
    private GLTFImport? gltf => node as GLTFImport;

    public override void _Ready()
    {
        if (path is null)
        {
            GD.PrintErr($"WorkerScript: No path found for worker {Worker.WorkerDefinition?.Name} from room {Worker.SourceRoomId} to {Worker.DestinationRoomId}");
            QueueFree();
            return;
        }
        node = Worker.WorkerDefinition?.Scene?.Instantiate();
        gltf?.AnimationPlayer?.Play("idle");
        node?.SetParent(this);

        if (path is not null)
        {
            if (towerState.Rooms.TryGetValue(path.TargetRoomId, out var destRoom) && destRoom.Elevation == worker.Elevation)
            {
                walkingToRoom = destRoom;
            }
            else
            {
                // assuming a sane route:
                // find the last transport that intersects the worker's elevation, and set that as the next transport to walk to.
                var nextStep = path.TransportsToTake.LastOrDefault(tp => towerState.Transports[tp.TransportId].IntersectsElevation(Worker.Elevation));
                walkingToTransport = nextStep is not null && towerState.Transports.TryGetValue(nextStep.TransportId, out var transport) ? transport : null;
            }
        }
    }

    public override void _EnterTree()
    {

    }

    public override void _Process(double delta)
    {
        if (walkingToRoom is null && walkingToTransport is null)
        {
            GD.PrintErr($"WorkerScript: No valid destination for worker {Worker.WorkerDefinition?.Name}");
            QueueFree();
            return;
        }

        var dir = (walkingToRoom?.FloorPosition ?? walkingToTransport?.HorizontalPosition ?? 0);
        Position = Position.MoveToward(new(dir, Position.Y, Position.Z), (float)delta * Worker.WorkerDefinition.MovementSpeed);

        if (Position.X == dir)
        {
            // worker reached checkpoint, stop and let the transport or room handle the next step
        }
    }
}
