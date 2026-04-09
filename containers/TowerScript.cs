using Godot;
using System;
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

        var fcs = this.AddedChild(new FloorsContainerScript(this));
        var rcs = this.AddedChild(new RoomsContainerScript(this));
        var tcs = this.AddedChild(new TransportsContainerScript(this));
        AddChild(new TowerRoomBuilderOverlay(this).Configured(overlay =>
        {
            overlay.OnRoomConstruct += rcs.OnRoomConstruct;
        }));
        AddChild(new TowerFloorBuilderOverlay(this).Configured(overlay =>
        {
            overlay.OnFloorExtend += fcs.OnFloorExtend;
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
