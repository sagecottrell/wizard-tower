using Godot;

namespace wizardtower.UIs.selector;

[Tool]
public partial class Selector : Node3D
{
    private uint increaseLeft;
    private uint increaseRight;
    private uint increaseUp;
    private uint increaseDown;

    [Signal]
    public delegate void OnAcceptEventHandler();
    [Signal]
    public delegate void OnCancelEventHandler();
    [Signal]
    public delegate void OnMouseEnteredEventHandler();
    [Signal]
    public delegate void OnMouseExitedEventHandler();

    [Export] public uint IncreaseLeft { get => increaseLeft; set => SetSize(left: value); }
    [Export] public uint IncreaseRight { get => increaseRight; set => SetSize(right: value); }
    [Export] public uint IncreaseUp { get => increaseUp; set => SetSize(up: value); }
    [Export] public uint IncreaseDown { get => increaseDown; set => SetSize(down: value); }

    [Export] private Node3D? vis;
    [Export] private Node3D? area3d;
    [Export] private BoxShape3D? box;

    public void InputEvent(Node _camera, InputEvent @event, Vector3 _position, Vector3 _normal, long _shapeIdx)
    {
        if (@event.IsActionReleased(InputMapConstants.RightClick))
            EmitSignalOnCancel();
        if (@event.IsActionReleased(InputMapConstants.LeftClick))
            EmitSignalOnAccept();
    }

    public void MouseEntered()
    {
        EmitSignalOnMouseEntered();
    }

    public void MouseExited()
    {
        EmitSignalOnMouseExited();
    }

    public void SetSize(uint? left = null, uint? right = null, uint? up = null, uint? down = null)
    {
        left ??= increaseLeft;
        right ??= increaseRight;
        up ??= increaseUp;
        down ??= increaseDown;

        increaseDown = (uint)down;
        increaseLeft = (uint)left;
        increaseRight = (uint)right;
        increaseUp = (uint)up;

        if (vis is not null)
        {
            vis.Scale = Vector3.One + new Vector3(increaseLeft + increaseRight, increaseUp + increaseDown, 0);
            vis.Position = new Vector3((int)increaseRight - increaseLeft, -(int)increaseDown * 2, 2) / 2;
        }
        if (area3d is not null)
        {
            area3d.Position = new Vector3((int)increaseRight - increaseLeft, (int)increaseUp - increaseDown + 1, 2) / 2;
        }
        if (box is not null)
        {
            box.Size = Vector3.One + new Vector3(increaseRight + increaseLeft, increaseUp + increaseDown, 0);
        }
    }
}
