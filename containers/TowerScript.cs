using Godot;
using System;
using wizardtower.actions;
using wizardtower.actions.ui;
using wizardtower.state;
using wizardtower.UIs;
using wizardtower.UIs.build_menu;
using wizardtower.UIs.tower;

namespace wizardtower.containers;

[GlobalClass]
public partial class TowerScript : Node3D
{
    [Export]
    public TowerState State { get; set; } = new();

    private TowerState PreviousState { get; set; } = new();

    [Export]
    public Node3D? Camera { get; set; }

    [Export]
    public Node? WalletContainer { get; set; }

    [Export]
    public UIManager? BuildMenu { get; set; }

    public override void _Ready()
    {
        State.EnsureGroundFloor();

        AddChild(new FloorsContainerScript(this));
        AddChild(new RoomsContainerScript(this));
        AddChild(new TransportsContainerScript(this));
        AddChild(new TowerRoomBuilderOverlay(this).Configured(overlay =>
        {
            overlay.OnRoomConstruct += Actions.BuyRoom;
        }));
        AddChild(new TowerFloorBuilderOverlay(this).Configured(overlay =>
        {
            overlay.OnFloorExtend += Actions.ExtendFloor;
            overlay.OnFloorReplace += Actions.ReplaceFloor;
            overlay.OnFloorConstruct += Actions.BuyFloor;
        }));
        AddChild(new TowerCameraDragScript(Camera, State));
        WalletContainer?.AddChild(new WalletUI(State));
        this.Child<UIManager>()?.ShowUI();
    }

    public override void _Process(double delta)
    {
        if (State != PreviousState && State.Compare(PreviousState))
            return;

        if (Camera is not null)
        {
            Camera.Position = Camera.Position with { Z = Math.Max(2, State.MaxBasement) + 3 };
        }

        PreviousState = State.Copy();
    }


    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event.IsActionPressed(InputMapConstants.OpenBuildMenu) && BuildMenu is not null)
        {
            if (BuildMenu.Child<BuildMenu>() is BuildMenu menu)
            {
                menu.QueueFree();
            }
            else if (SceneLoader.TryLoadScene<BuildMenu>(out var bm))
            {
                BuildMenu.AddChild(bm);
                bm.SetTower(State);
            }
        }
        else if (@event.IsActionPressed(InputMapConstants.Cancel))
            UIActions.Cancel(new());
    }
}
