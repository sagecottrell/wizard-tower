using Godot;

namespace wizardtower;

[Tool]
public partial class FloorBackgroundTile : Node3D
{
    public int Index;

    [Export(PropertyHint.ExpEasing)]
    public float EaseIn { get; set; }

    [Export]
    public float InTime { get; set; }

    [Signal]
    public delegate void OnMouseEnteredEventHandler(FloorBackgroundTile sender);

    [Signal]
    public delegate void OnMouseExitedEventHandler(FloorBackgroundTile sender);


    // OnDestroy

    public void OnCreate()
    {
        Position = this.TowerCoordToNodePosition(y: -1);
        var lerp = new PosLerpHelper()
        {
            InTime = InTime,
            EaseIn = EaseIn,
            Prev = Position,
            Next = this.TowerCoordToNodePosition(y: 0),
        };
        AddChild(lerp);
        lerp.OnComplete += lerp.QueueFree;
    }


    public void MouseEntered() => EmitSignalOnMouseEntered(this);

    public void MouseExited() => EmitSignalOnMouseExited(this);
}
