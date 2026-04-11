using Godot;
using PhantomCamera;
using wizardtower.state;

namespace wizardtower.UIs;

public partial class TowerCameraDragScript(Node3D camera, TowerState tower) : Node
{
    private bool tracking;
    private Vector2 dist;
    private readonly PhantomCamera3D phantomCamera = camera.AsPhantomCamera3D();

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.IsActionPressed(InputMapConstants.RightClick))
            {
                tracking = true;
                dist = default;
                GetViewport().SetInputAsHandled();
            }
            else if (tracking && mouseButton.IsActionReleased(InputMapConstants.RightClick)) 
            {
                tracking = false;
                if (!dist.IsZeroApprox())
                    GetViewport().SetInputAsHandled();
            }
        }
        if (tracking && @event is InputEventMouseMotion motion)
        {
            var factor = phantomCamera.Size / GetWindow().Size.Y;
            dist += motion.Relative;
            camera.Position += new Vector3(0f, motion.Relative.Y, 0f) * factor;
            camera.Position = camera.Position.Clamp(-(int)tower.MaxBasement, tower.MaxHeight);
        }
    }
}
