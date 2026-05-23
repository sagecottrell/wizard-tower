using Godot;
using wizardtower.actions.ui;
using wizardtower.events.handlers;
using wizardtower.events.interfaces;
using wizardtower.events.Room.ui;
using wizardtower.events.ui;
using wizardtower.state;
using wizardtower.UIs.transport_details;

namespace wizardtower.UIs.room_details;

public partial class RoomDetailsUI(TowerState tower) : CanvasLayer, IUserInterface
{
    private RoomState? RoomState { get; set; }
    private Control ui = new VBoxContainer();

    public override void _Ready()
    {
        AddChild(new PanelContainer()
        {
            AnchorRight = 1,
            AnchorLeft = 1,
            PivotOffsetRatio = new Vector2(1, 0),
            GrowHorizontal = Control.GrowDirection.Begin,
        }.WithChild(ui));
    }

    public override void _EnterTree()
    {
        RoomEvents.UI.Selected += _onRoomSelected;
        RoomEvents.UI.Deselected += _onRoomDeselected;
        FloorEvents.UI.ConstructionSelected += _event_hide;
        RoomEvents.UI.ConstructionSelected += _event_hide;
        TransportEvents.UI.ConstructionSelected += _event_hide;
        GeneralEvents.ShowedUI += _onShowedUI;
    }

    public override void _ExitTree()
    {
        RoomEvents.UI.Selected -= _onRoomSelected;
        RoomEvents.UI.Deselected -= _onRoomDeselected;
        FloorEvents.UI.ConstructionSelected -= _event_hide;
        RoomEvents.UI.ConstructionSelected -= _event_hide;
        TransportEvents.UI.ConstructionSelected -= _event_hide;
        GeneralEvents.ShowedUI -= _onShowedUI;
    }

    private void _onShowedUI(ShowedUIEvent @event)
    {
        if (RoomState is null)
            return;
        switch (@event.UserInterface)
        {
            case TransportDetailsUI:
                UIActions.DeselectRoom(new(tower, RoomState));
                break;
        }
    }

    private void _reset()
    {
        RoomState = null;
        Visible = false;
    }

    private void _onRoomDeselected(RoomDeselectedEvent @event)
    {
        if (@event.RoomState != RoomState)
            return;
        _reset();
    }

    private void _event_hide(IEvent @event)
    {
        if (RoomState is null)
            return;
        UIActions.DeselectRoom(new(tower, RoomState));
    }

    private void _onRoomSelected(RoomSelectedEvent @event)
    {
        if (@event.TowerState != tower)
            return;

        if (@event.RoomState == RoomState)
        {
            UIActions.DeselectRoom(new(tower, RoomState));
            return;
        }
        if (RoomState is not null)
            UIActions.DeselectRoom(new(tower, RoomState));

        RoomState = @event.RoomState;
        Visible = true;

        if (ui.Child<RichTextLabel>() is not RichTextLabel rtl)
            rtl = ui.AddedChild(new RichTextLabel
            {
                FitContent = true,
                CustomMinimumSize = new Vector2(200, 0),
                AutowrapMode = TextServer.AutowrapMode.Off,
                BbcodeEnabled = true,
            });
        rtl.Text = "";
        _pushText(rtl);

        // if (ui.Child<Button>())

        GeneralEvents.OnShowedUI(new(this));
    }

    private void _pushText(RichTextLabel rtl)
    {
        if (RoomState is null)
            return;
        rtl.AppendText($"Selected Room #{RoomState.Id}\n");
        rtl.AppendText($"{RoomState.Definition.Name} {rtl.LineHeightImage(RoomState.Definition.Icon)}\n");
        rtl.AppendText($"Floor {RoomState.Elevation}, Room {Mathf.Abs(RoomState.FloorPosition),3:D3}{(RoomState.FloorPosition < 0 ? "L" : "R")}\n");
        if (RoomState.StoredItems.Count > 0)
        {
            rtl.AddText("Stored items:\n");
            rtl.PushList(0, RichTextLabel.ListType.Dots, false);
            foreach (var (def, amount) in RoomState.StoredItems)
            {
                rtl.AppendText($"{amount} {def.Name} {rtl.LineHeightImage(def.Icon)}\n");
            }
            rtl.Pop();
        }
    }
}
