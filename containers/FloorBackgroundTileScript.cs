using Godot;

namespace wizardtower.containers;

[Tool]
public partial class FloorBackgroundTileScript : Node3D
{
    public int Index;

    [Export(PropertyHint.ExpEasing)]
    public float EaseIn { get; set; } = 1f;

    [Export]
    public float InTime { get; set; } = .25f;

    [Export]
    public float StartingZ { get; set; } = -2f;

    [Export]
    public int InDirection { get; set; } = -1;

    [Signal]
    public delegate void OnMouseEnteredEventHandler(FloorBackgroundTileScript sender);

    [Signal]
    public delegate void OnMouseExitedEventHandler(FloorBackgroundTileScript sender);

    [Export]
    public Node3D? Wall { get; set; }

    public void WallVisible(bool visible)
    {
        if (Wall is not null)
            Wall.Visible = visible;
    }


    // OnDestroy

    public void OnCreate()
    {
        Position = this.TowerCoordToNodePosition(y: InDirection, z: StartingZ);
        var lerp = new PosLerpHelper()
        {
            InTime = InTime,
            EaseIn = EaseIn,
            Prev = Position,
            Next = this.TowerCoordToNodePosition(y: 0, z: StartingZ),
        };
        AddChild(lerp);
        lerp.OnComplete += () =>
        {
            lerp.QueueFree();
            Position = this.TowerCoordToNodePosition(y: 0, z: 0);
        };
    }


    public void MouseEntered() => EmitSignalOnMouseEntered(this);

    public void MouseExited() => EmitSignalOnMouseExited(this);
}
