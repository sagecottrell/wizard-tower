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


    // OnDestroy

    // OnCreate

    public void OnCreate()
    {
        var lerp = new PosLerpHelper()
        {
            InTime = InTime,
            EaseIn = EaseIn,
            Prev = this.TowerCoordToNodePosition(y: -1),
            Next = this.TowerCoordToNodePosition(y: 0),
        };
        AddChild(lerp);
        lerp.OnComplete += lerp.QueueFree;
    }

}
