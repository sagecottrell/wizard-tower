using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using Godot;
using wizardtower.actions.ui;
using wizardtower.events.handlers;
using wizardtower.events.Interface;
using wizardtower.events.interfaces;
using wizardtower.events.Room;
using wizardtower.events.Room.ui;
using wizardtower.state;
using wizardtower.UIs.transport_details;

namespace wizardtower.UIs.room_details;

public partial class RoomDetailsUI(TowerState tower) : CanvasLayer, IUserInterface
{
    private RoomState? RoomState { get; set; }
    private Control ui = new VBoxContainer();

    public override void _Ready()
    {
        Name = nameof(RoomDetailsUI);
        AddChild(new PanelContainer()
        {
            AnchorRight = 1,
            AnchorLeft = 1,
            PivotOffsetRatio = new Vector2(1, 0),
            GrowHorizontal = Control.GrowDirection.Begin,
        }.WithChild(new MarginContainer()
        {
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter,
        }.WithChild(ui)));
    }

    public override void _EnterTree()
    {
        RoomEvents.Ui.Selected += _onRoomSelected;
        RoomEvents.Ui.Deselected += _onRoomDeselected;
        RoomEvents.ProducedResources += _onProducedResources;
        FloorEvents.Ui.ConstructionSelected += _event_hide;
        RoomEvents.Ui.ConstructionSelected += _event_hide;
        TransportEvents.Ui.ConstructionSelected += _event_hide;
        InterfaceEvents.Showed += _onShowedUI;
    }

    public override void _ExitTree()
    {
        RoomEvents.Ui.Selected -= _onRoomSelected;
        RoomEvents.Ui.Deselected -= _onRoomDeselected;
        RoomEvents.ProducedResources -= _onProducedResources;
        FloorEvents.Ui.ConstructionSelected -= _event_hide;
        RoomEvents.Ui.ConstructionSelected -= _event_hide;
        TransportEvents.Ui.ConstructionSelected -= _event_hide;
        InterfaceEvents.Showed -= _onShowedUI;
    }

    private void _onShowedUI(InterfaceShowedEvent @event)
    {
        if (RoomState is null)
            return;
        switch (@event.UserInterface)
        {
            case TransportDetailsUI:
                UIActions.DeselectRoom(new(tower, RoomState) { Source = @event });
                break;
        }
    }

    private void _reset()
    {
        RoomState = null;
        Visible = false;
        ui.FreeChildren();
    }

    private void _onRoomDeselected(RoomDeselectedEvent @event)
    {
        _reset();
    }

    private void _event_hide(IEvent @event)
    {
        if (RoomState is null)
            return;
        UIActions.DeselectRoom(@event.RoomDeselectingEvent(tower, RoomState));
    }

    private void _onProducedResources(RoomProducedResourcesEvent ev)
    {
        if (ev.RoomState != RoomState)
            return;
        _pushText();
    }

    private void _onRoomSelected(RoomSelectedEvent @event)
    {
        if (@event.TowerState != tower)
            return;

        if (@event.RoomState.Id == RoomState?.Id)
        {
            UIActions.DeselectRoom(@event.RoomDeselectingEvent(tower, RoomState));
            return;
        }

        if (RoomState is not null)
            UIActions.DeselectRoom(@event.RoomDeselectingEvent(tower, RoomState));

        RoomState = @event.RoomState;
        Visible = true;

        List<CheckBox> checkboxes = [];
        List<RichTextLabel> rtls = [];

        _pushText();

        ui.AddChild(new Label() { Text = "Stored Items:" });

        if (RoomState.Definition.RelatedItems is { } outputs)
        {
            foreach (var def in outputs)
            {

                ui.AddedChild(new CheckBox()
                {
                    Visible = false,
                    Icon = def.Icon,
                    Text = $"{RoomState.StoredItems.GetOrDefault(def)} {def.Name}"
                }.Configured(b =>
                {
                    b.Pressed += () => { };
                    checkboxes.Add(b);
                }));
                ui.AddedChild(this.RTLWithGoodDefaultSettings().Configured(rtl =>
                {
                    rtl.Text = $"{RoomState.StoredItems.GetOrDefault(def)} {def.Name} {rtl.LineHeightImage(def.Icon)}";
                    rtls.Add(rtl);
                }));
            }
        }

        ui.AddChild(new HSeparator());

        ui.AddChild(new GridContainer()
        {
            Columns = 2,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter,
        }.Configured(grid =>
        {
            grid.AddChild(new Button()
            {
                TooltipText = "Add worker path",
                Icon = ResourceLoader.Load<Texture2D>("uid://bmjgmx6fxuqgx"),
            }.Configured(b =>
            {
                b.Pressed += () => UIActions.DeselectRoom(@event.RoomDeselectingEvent(tower, RoomState));
            }));
        }));

        InterfaceEvents.OnShowed(new(this));
    }

    private void _pushText()
    {
        if (RoomState is null)
            return;

        var rtl = ui.EnsureChild("rtl", () => new RichTextLabel
        {
            FitContent = true,
            CustomMinimumSize = new Vector2(200, 0),
            AutowrapMode = TextServer.AutowrapMode.Off,
            BbcodeEnabled = true,
        });
        rtl.Text = "";

        rtl.AppendText($"Selected Room #{RoomState.Id}\n");
        rtl.AppendText($"{RoomState.Definition.Name} {rtl.LineHeightImage(RoomState.Definition.Icon)}\n");
        rtl.AppendText($"Floor {RoomState.Elevation}, Room {Mathf.Abs(RoomState.FloorPosition),3:D3}{(RoomState.FloorPosition < 0 ? "L" : "R")}\n");
        if (RoomState.Warehouse is { } wh)
            rtl.AppendText($"Warehouse {wh.Name} {rtl.LineHeightImage(wh.Icon)}");
        if (!RoomState.HasSufficientMaterials())
            rtl.AppendText("Awaiting Materials\n");
        if (!RoomState.HasSufficientWorkers())
            rtl.AppendText("Awaiting Workers\n");
    }
}
