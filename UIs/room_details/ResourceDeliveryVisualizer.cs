using Godot;
using System.Linq;
using wizardtower.state;

namespace wizardtower.UIs.room_details;

[Tool]
[GlobalClass]
public partial class ResourceDeliveryVisualizer : Path3D
{
    private StandardMaterial3D multimeshMaterial;
    private MultiMeshInstance3D multiMeshInstance;
    private StandardMaterial3D csgMaterial;
    private QuadMesh quadMesh;
    private float timeElapsed;

    private uint fromRoomId;
    private Color color = Colors.Red;
    private TowerState? towerState;
    private Vector2 itemScale = new(0.5f, 0.5f);
    private RoomStateWorkerPath? workerPath;
    private float timeOffset;

    [Export]
    public float ItemDistance { get; set; } = 3.0f;
    [Export]
    public float Speed { get; set; } = 2.0f;
    [Export(PropertyHint.ExpEasing)]
    public float Easing { get; set; } = 0.147f;
    [Export]
    public Vector2 ItemScale { get => itemScale; set => SetItemScale(value); }
    [Export]
    public RoomStateWorkerPath? WorkerPath { get => workerPath; set => SetWorkerPath(value); }
    [Export]
    public uint FromRoomId { get => fromRoomId; set => SetFromRoomId(value); }
    [Export]
    public Color Color { get => color; set => SetColor(value); }
    [Export]
    public TowerState? TowerState { get => towerState; set => SetTowerState(value); }
    [Export]
    public float TimeOffset { get => timeOffset; set => SetTimeOffset(value); }

    public void SetWorkerPath(RoomStateWorkerPath? workerPath)
    {
        this.workerPath = workerPath;
        if (workerPath?.ItemDefinition?.Icon != null)
            multimeshMaterial.AlbedoTexture = workerPath.ItemDefinition.Icon;
    }

    public void SetFromRoomId(uint fromRoomId)
    {
        this.fromRoomId = fromRoomId;
    }

    public void SetColor(Color color)
    {
        this.color = color;
        csgMaterial.AlbedoColor = color;
    }

    public void SetTowerState(TowerState? towerState)
    {
        this.towerState = towerState;
    }

    public void SetItemScale(Vector2 itemScale)
    {
        this.itemScale = itemScale;
        quadMesh.Size = itemScale;
    }

    public void SetTimeOffset(float timeOffset)
    {
        timeElapsed += timeOffset - this.timeOffset;
        this.timeOffset = timeOffset;
    }

    public ResourceDeliveryVisualizer()
    {
        multimeshMaterial = new()
        {
            BillboardMode = BaseMaterial3D.BillboardModeEnum.Enabled,
            BillboardKeepScale = true,
        };
        quadMesh = new QuadMesh()
        {
            Size = ItemScale,
            Material = multimeshMaterial,
        };
        multiMeshInstance = new()
        {
            Multimesh = new MultiMesh()
            {
                TransformFormat = MultiMesh.TransformFormatEnum.Transform3D,
                Mesh = quadMesh,
            },
        };
        csgMaterial = new()
        {
            AlbedoColor = Colors.Red,
        };
        Curve = new() { BakeInterval = 0.1f };
    }

    public override void _Ready()
    {
        AddChild(multiMeshInstance);
        AddChild(new CsgPolygon3D()
        {
            Polygon = [
                new(.01f, -.1f), new(.01f, -.05f),
                new(.1f, -0.05f), new(.1f, -.1f),
                ],
            Mode = CsgPolygon3D.ModeEnum.Path,
            PathInterval = 0.01f,
            PathSimplifyAngle = 80,
            PathLocal = true,
            PathNode = GetPath(),
            SmoothFaces = true,
            Material = csgMaterial,
        });
    }

    public override void _Process(double delta)
    {
        timeElapsed = Mathf.PosMod(timeElapsed + (float)delta, ItemDistance / Speed);
        _updateMultimesh();
    }

    private void _updateMultimesh()
    {
        if (WorkerPath?.ItemDefinition == null) 
            return;
        var pathLength = Curve.GetBakedLength();
        var count = Mathf.CeilToInt(pathLength / ItemDistance);
        var mm = multiMeshInstance.Multimesh;
        mm.InstanceCount = count;

        for (int i = 0; i < count; i++)
        {
            var dist = timeElapsed * Speed + ItemDistance * i;
            if (dist > pathLength)
            {
                mm.SetInstanceTransform(i, new Transform3D().ScaledLocal(Vector3.Zero));
                continue;
            }
            var pos = Curve.SampleBakedWithRotation(dist, true, true);
            float factor = 1;
            if (dist + Speed > pathLength)
                factor = (pathLength - dist) / Speed;
            else if (dist < Speed)
                factor = dist / Speed;
            if (factor != 1)
                pos = pos.ScaledLocal(Vector3.One * Mathf.Ease(factor, Easing));
            mm.SetInstanceTransform(i, pos);
        }
    }

    public void SetupPath()
    {
        if (TowerState is null || WorkerPath is null || WorkerPath.TransportsToTake is null)
            return;
        if (!TowerState.Rooms.TryGetValue(FromRoomId, out var fromRoom) || !TowerState.Rooms.TryGetValue(WorkerPath.TargetRoomId, out var toRoom))
            return;
        var fromPos = new Vector3(fromRoom.FloorPosition, fromRoom.Elevation, 0);
        var toPos = new Vector3(toRoom.FloorPosition, toRoom.Elevation, 0);
        Curve.ClearPoints();
        Curve.AddPoint(new());
        var currentFloor = fromRoom.Elevation;
        foreach (var tt in WorkerPath.TransportsToTake)
        {
            if (!TowerState.Transports.TryGetValue(tt.TransportId, out var transport))
                continue;
            Curve.AddPoint(new Vector3(transport.HorizontalPosition, currentFloor, 0) - fromPos);
            Curve.AddPoint(new Vector3(transport.HorizontalPosition, tt.Elevation, 0) - fromPos);
            currentFloor = tt.Elevation;
        }
        Curve.AddPoint(toPos - fromPos);
    }
}
