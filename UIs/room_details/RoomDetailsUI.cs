using System.Collections.Generic;
using System.Linq;
using Godot;
using wizardtower.actions.ui;
using wizardtower.events.handlers;
using wizardtower.events.Interface;
using wizardtower.events.interfaces;
using wizardtower.events.Room;
using wizardtower.events.Room.ui;
using wizardtower.resource_types;
using wizardtower.state;
using wizardtower.UIs.transport_details;

namespace wizardtower.UIs.room_details;

public partial class RoomDetailsUI(TowerState tower) : CanvasLayer, IUserInterface
{
    private RoomState? RoomState { get; set; }
    private Control ui = new VBoxContainer();

    private readonly List<CustomCheckBox> checkboxes = [];
    private readonly List<RichTextLabel> rtls = [];

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
        RoomEvents.Ui.DeliveryPlanningCompleted += _onDeliveryCompleted;
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
        RoomEvents.Ui.DeliveryPlanningCompleted -= _onDeliveryCompleted;
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

    private void _onDeliveryCompleted(RoomDeliveryPlanningCompletedEvent ev)
    {
        _endAddWorkerPath();
    }

    #region Room Selected

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
        checkboxes.Clear();
        rtls.Clear();

        _pushText();

        ui.AddChild(new Label() { Text = "Stored Items:" });

        if (RoomState.PossibleOutputs is { } outputs)
        {
            // outputs have checkboxes that are hidden by default, and can be toggled to show by clicking the "Add worker path" button
            foreach (var def in outputs)
            {
                ui.AddedChild(new CustomCheckBox(def)
                {
                    Visible = false,
                    Icon = def.Icon,
                    Text = $"{RoomState.StoredItems.GetOrDefault(def)} {def.Name}",
                }.Configured(b =>
                {
                    checkboxes.Add(b);
                    b.Toggled += (_) => _checkboxToggled();
                }));
                ui.AddedChild(this.RTLWithGoodDefaultSettings().Configured(rtl =>
                {
                    rtl.Text = $"{RoomState.StoredItems.GetOrDefault(def)} {def.Name} {rtl.LineHeightImage(def.Icon)}";
                    rtls.Add(rtl);
                }));
            }
        }
        if (RoomState.Inputs is { } inputs)
        {
            // inputs are just text, no checkboxes
            foreach (var def in inputs)
            {
                ui.AddedChild(this.RTLWithGoodDefaultSettings().Configured(rtl =>
                {
                    rtl.Text = $"{RoomState.StoredItems.GetOrDefault(def)} {def.Name} {rtl.LineHeightImage(def.Icon)}";
                }));
            }
        }

        ui.AddChild(new HSeparator());

        ui.AddChild(new HFlowContainer()
        {
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter,
            LastWrapAlignment = FlowContainer.LastWrapAlignmentMode.Center,
        }.Configured(grid =>
        {
            grid.AddChild(new Button()
            {
                TooltipText = "Add worker path",
                Icon = ResourceLoader.Load<Texture2D>("uid://bmjgmx6fxuqgx"),
            }.Configured(b =>
            {
                b.Pressed += _startAddWorkerPath;
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

    #endregion


    #region UI Callbacks

    void _startAddWorkerPath()
    {
        foreach (var cb in checkboxes)
        {
            cb.Visible = true;
            if (checkboxes.Count == 1)
            {
                cb.SetPressedNoSignal(true);
                _checkboxToggled();
            }
            else
            {
                cb.SetPressedNoSignal(false);
            }
        }
        foreach (var rtl in rtls)
            rtl.Visible = false;
    }

    void _endAddWorkerPath()
    {
        foreach (var cb in checkboxes)
        {
            cb.Visible = false;
        }
        foreach (var rtl in rtls)
            rtl.Visible = true;
    }

    /// <summary>
    /// when an item checkbox is toggled, collect all the checked boxes from the button group and emit a TryDeliveryPlanning event
    /// </summary>
    void _checkboxToggled()
    {
        if (RoomState is null)
            return;
        var selectedItems = checkboxes
            .Where(c => c.ButtonPressed)
            .Select(c => c.ItemDefinition);
        if (RoomEvents.Ui.TryDeliveryPlanningStarting(new(tower, RoomState, [.. selectedItems]), out var ev))
            RoomEvents.Ui.OnDeliveryPlanningStarted(ev);
    }

    #endregion


    #region CheckBox custom subclass

    private partial class CustomCheckBox : CheckBox
    {
        public CustomCheckBox(ItemDefinition itemDefinition)
        {
            ItemDefinition = itemDefinition;
            ExpandIcon = true;
        }

        public ItemDefinition ItemDefinition { get; }
    }

    #endregion
}
