using Godot;
using wizardtower.actions.ui;
using wizardtower.events.handlers;
using wizardtower.events.interfaces;
using wizardtower.events.Transport.ui;
using wizardtower.events.ui;
using wizardtower.state;
using wizardtower.UIs.room_details;

namespace wizardtower.UIs.transport_details;

public partial class TransportDetailsUI(TowerState tower) : CanvasLayer, IUserInterface
{
    private TransportState? Transport { get; set; }
    private Control ui = new PanelContainer()
    {
        AnchorRight = 1,
        AnchorLeft = 1,
        PivotOffsetRatio = new Vector2(1, 0),
        GrowHorizontal = Control.GrowDirection.Begin,
    };

    public override void _Ready()
    {
        AddChild(ui);
    }

    public override void _EnterTree()
    {
        TransportEvents.UI.TransportSelected += _onTransportSelected;
        TransportEvents.UI.TransportDeselected += _onTransportDeselected;
        FloorEvents.UI.FloorConstructionSelected += _event_hide;
        RoomEvents.UI.RoomConstructionSelected += _event_hide;
        TransportEvents.UI.TransportConstructionSelected += _event_hide;
        GeneralEvents.ShowedUI += _onShowedUI;
    }

    public override void _ExitTree()
    {
        TransportEvents.UI.TransportSelected -= _onTransportSelected;
        TransportEvents.UI.TransportDeselected -= _onTransportDeselected;
        FloorEvents.UI.FloorConstructionSelected -= _event_hide;
        RoomEvents.UI.RoomConstructionSelected -= _event_hide;
        TransportEvents.UI.TransportConstructionSelected -= _event_hide;
        GeneralEvents.ShowedUI -= _onShowedUI;
    }

    private void _onTransportDeselected(TransportDeselectedEvent @event)
    {
        if (@event.Transport != Transport)
            return;
        _reset();
    }

    private void _reset()
    {
        Transport = null;
        Visible = false;
    }

    private void _event_hide(IEvent @event)
    {
        if (Transport is null)
            return;
        UIActions.DeselectTransport(new(tower, Transport));
    }

    private void _onShowedUI(ShowedUIEvent @event)
    {
        if (Transport is null)
            return;
        switch (@event.UserInterface)
        {
            case RoomDetailsUI:
                UIActions.DeselectTransport(new(tower, Transport));
                break;
        }
    }

    private void _onTransportSelected(TransportSelectedEvent @event)
    {
        if (@event.TowerState != tower)
            return;
        if (@event.Transport == Transport)
        {
            UIActions.DeselectTransport(new(tower, Transport));
            return;
        }
        Transport = @event.Transport;
        Visible = true;

        var text = $"Selected Transport #{Transport.Id}: {Transport.Definition.Name}  {ui.LineHeightImage(Transport.Definition.Icon)}";

        if (ui.Child<RichTextLabel>() is not RichTextLabel rtl)
            rtl = ui.AddedChild(new RichTextLabel
            {
                FitContent = true,
                CustomMinimumSize = new Vector2(200, 0),
                AutowrapMode = TextServer.AutowrapMode.Off,
                BbcodeEnabled = true,
            });
        rtl.Text = text;
        GeneralEvents.OnShowedUI(new(this));
    }
}
