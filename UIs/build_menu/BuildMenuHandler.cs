using Godot;
using System;
using wizardtower.actions.ui;
using wizardtower.events.ui;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;

public partial class BuildMenuHandler(TowerState tower) : CanvasLayer, IUserInterface
{
    private BuildMenu buildMenu = SceneLoader.TryLoadScene<BuildMenu>(out var bm) ? bm.Configured(x => x.SetTower(tower)) : throw new Exception("Failed to load BuildMenu scene");

    public override void _Ready()
    {
        AddChild(buildMenu);
        buildMenu.Visible = false;
    }

    public override void _EnterTree()
    {
        GlobalSignals.Singleton.OnHidingUI += _onHidingUI;
        GlobalSignals.Singleton.OnShowedUI += _onShowedUI;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnHidingUI -= _onHidingUI;
        GlobalSignals.Singleton.OnShowedUI -= _onShowedUI;
    }

    private void _onHidingUI(HidingUIEvent @event)
    {
        buildMenu.Visible = false;
    }

    private void _onShowedUI(ShowedUIEvent @event)
    {
        if (@event.UserInterface != this)
            return;
        buildMenu.Visible = true;
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event.IsActionPressed(InputMapConstants.OpenBuildMenu))
        {
            if (buildMenu.Visible)
                UIActions.Hide(new(this) { Input = @event });
            else
                UIActions.ShowUI(new(this) { Input = @event });
        }
        else if (@event.IsActionPressed(InputMapConstants.Cancel))
            UIActions.Hide(new(this) { Input = @event });
    }

}
