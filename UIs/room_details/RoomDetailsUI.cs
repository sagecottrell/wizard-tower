using Godot;
using System.Collections.Generic;
using wizardtower.actions.ui;
using wizardtower.events.interfaces;
using wizardtower.events.ui;
using wizardtower.state;
using wizardtower.UIs.transport_details;

namespace wizardtower.UIs.room_details;

public partial class RoomDetailsUI(TowerState tower) : CanvasLayer, IUserInterface
{
    private RoomState? Room { get; set; }
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
        GlobalSignals.Singleton.OnRoomSelected += _onRoomSelected;
        GlobalSignals.Singleton.OnRoomDeselected += _onRoomDeselected;
        GlobalSignals.Singleton.OnFloorConstructionSelected += _event_hide;
        GlobalSignals.Singleton.OnRoomConstructionSelected += _event_hide;
        GlobalSignals.Singleton.OnTransportConstructionSelected += _event_hide;
        GlobalSignals.Singleton.OnShowedUI += _onShowedUI;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnRoomSelected -= _onRoomSelected;
        GlobalSignals.Singleton.OnRoomDeselected -= _onRoomDeselected;
        GlobalSignals.Singleton.OnFloorConstructionSelected -= _event_hide;
        GlobalSignals.Singleton.OnRoomConstructionSelected -= _event_hide;
        GlobalSignals.Singleton.OnTransportConstructionSelected -= _event_hide;
        GlobalSignals.Singleton.OnShowedUI -= _onShowedUI;
    }

    private void _onShowedUI(ShowedUIEvent @event)
    {
        if (Room is null)
            return;
        switch (@event.UserInterface)
        {
            case TransportDetailsUI:
                UIActions.DeselectRoom(new(tower, Room));
                break;
        }
    }

    private void _reset()
    {
        Room = null;
        Visible = false;
    }

    private void _onRoomDeselected(RoomDeselectedEvent @event)
    {
        if (@event.Room != Room)
            return;
        _reset();
    }

    private void _event_hide(IEvent @event)
    {
        if (Room is null)
            return;
        UIActions.DeselectRoom(new(tower, Room));
    }

    private void _onRoomSelected(RoomSelectedEvent @event)
    {
        if (@event.TowerState != tower)
            return;

        if (@event.Room == Room)
        {
            UIActions.DeselectRoom(new(tower, Room));
            return;
        }
        Room = @event.Room;
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

        GlobalSignals.ShowedUI(new(this));
    }

    private void _pushText(RichTextLabel rtl)
    {
        if (Room is null)
            return;
        rtl.AppendText($"Selected Room #{Room.Id}\n");
        rtl.AppendText($"{Room.Definition.Name} {rtl.LineHeightImage(Room.Definition.Icon)}\n");
        rtl.AppendText($"Floor {Room.Elevation}, Room {Mathf.Abs(Room.FloorPosition),3:D3}{(Room.FloorPosition < 0 ? "L" : "R")}\n");
        this.Debug($"Stored items: {Room.StoredItems.Count}");
        if (Room.StoredItems.Count > 0)
        {
            rtl.AddText("Stored items:\n");
            rtl.PushList(0, RichTextLabel.ListType.Dots, false);
            foreach (var (def, amount) in Room.StoredItems)
            {
                rtl.AppendText($"{amount} {def.Name} {rtl.LineHeightImage(def.Icon)}\n");
            }
            rtl.Pop();
        }
    }
}
