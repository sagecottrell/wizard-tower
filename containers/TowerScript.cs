using Godot;
using System;
using wizardtower.state;
using wizardtower.UIs;
using wizardtower.UIs.build_menu;
using wizardtower.UIs.room_details;
using wizardtower.UIs.tower;
using wizardtower.UIs.transport_details;

namespace wizardtower.containers;

[GlobalClass]
public partial class TowerScript : Node3D
{
    [Export]
    public TowerState State { get; set; } = new();

    private TowerState PreviousState { get; set; } = new();

    [Export]
    public Node3D? Camera { get; set; }

    public override void _Ready()
    {
        State.EnsureGroundFloor();

        AddChild(new FloorsContainerScript(this));
        AddChild(new RoomsContainerScript(this));
        AddChild(new TransportsContainerScript(this));
        AddChild(new TowerRoomBuilderOverlay(this));
        AddChild(new TowerFloorBuilderOverlay(this));
        AddChild(new TowerTransportBuilderOverlay(this));
        AddChild(new TowerCameraDragScript(Camera, State));
        AddChild(new RoomDetailsUI(State));
        AddChild(new TransportDetailsUI(State));
        AddChild(new BuildMenuHandler(State));
        AddChild(new WalletUI(State));
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
