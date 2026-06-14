using Godot;
using System.Collections.Generic;
using System.Linq;

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
    [Export] private Material? selectorMaterial;
    [Export] private Material? hoverMaterial;
    [Export] private CollisionShape3D? shape;
    [Export] private MeshInstance3D? mesh;

    private uint isHovered;

    private Dictionary<Selector, bool> _hoverChain = [];

    public override void _Ready()
    {
        if (selectorMaterial is not null && hoverMaterial is not null)
        {
            var copy = selectorMaterial.RDuplicate();
            copy.NextPass = hoverMaterial;
            hoverMaterial = copy;
        }

        box = box?.RDuplicate();
        if (shape is not null)
            shape.Shape = box;

        SetSize();
    }

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
        _hover();
    }

    public void MouseExited()
    {
        EmitSignalOnMouseExited();
        _unhover();
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

    public void AddHoverForwarding(params Selector[] propagateTo)
    {
        foreach (var other in propagateTo)
        {
            _hoverChain[other] = isHovered > 0;
            if (isHovered > 0)
                other._hover();
        }
    }

    public void HoverIfOthers(params Selector[] from)
    {
        foreach (var other in from)
        {
            other._hoverChain[this] = false;
            if (other.isHovered > 0)
                _hover();
        }
    }

    private void _hover()
    {
        isHovered++;
        if (mesh is not null)
            mesh.MaterialOverride = hoverMaterial;

        if (_hoverChain.Count == 0)
            return;
        foreach (var (other, done) in _hoverChain.ToList())
            if (!done)
            {
                _hoverChain[other] = true;
                other._hover();
            }
    }

    private void _unhover()
    {
        isHovered--;
        if (isHovered == 0 && mesh is not null)
            mesh.MaterialOverride = null;

        if (_hoverChain.Count == 0)
            return;
        foreach (var other in _hoverChain.Keys.ToList())
        {
            _hoverChain[other] = false;
            other._unhover();
        }
    }
}
