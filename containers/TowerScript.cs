using Godot;
using System;
using wizardtower.actions;
using wizardtower.state;
using wizardtower.UIs.build_menu;

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
    public BuildMenu? BuildMenu { get; set; }

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

        BuildMenu?.SetTower(State);
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
}
