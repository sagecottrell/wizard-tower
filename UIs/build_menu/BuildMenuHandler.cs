using Godot;
using System;
using wizardtower.actions.ui;
using wizardtower.events.handlers;
using wizardtower.events.Interface;
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
        InterfaceEvents.Hiding += _onHidingUI;
        InterfaceEvents.Showed += _onShowedUI;
    }

    public override void _ExitTree()
    {
        InterfaceEvents.Hiding  -= _onHidingUI;
        InterfaceEvents.Showed -= _onShowedUI;
    }

    private void _onHidingUI(InterfaceHidingEvent @event)
    {
        buildMenu.Visible = false;
    }

    private void _onShowedUI(InterfaceShowedEvent @event)
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
                UIActions.Hide(new(this) { Source = new UserEvent(@event) });
            else
                UIActions.ShowUI(new(this) { Source = new UserEvent(@event) });
        }
        else if (@event.IsActionPressed(InputMapConstants.Cancel))
            UIActions.Hide(new(this) { Source = new UserEvent(@event) });
    }

}
