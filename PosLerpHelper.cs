using Godot;

namespace wizardtower;

public partial class PosLerpHelper : Node
{
    private double _inTime;

    public Vector3 Prev;
    public Vector3 Next;

    public float EaseIn { get; set; }
    public float InTime { get; set; }

    [Signal]
    public delegate void OnCompleteEventHandler();

    public override void _Process(double delta)
    {
        if (_inTime >= InTime)
            return;

        _inTime = Mathf.Clamp(_inTime + delta, 0, InTime);
        var p = (float)Mathf.Ease(_inTime / InTime, EaseIn);
        switch (GetParent())
        {
            case Node3D n3d:
                {
                    n3d.Position = Prev.Lerp(Next, p);
                    break;
                }
        }

        if (_inTime >= InTime)
        {
            EmitSignalOnComplete();
        }
    }
}
