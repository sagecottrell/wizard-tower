using Godot;
using Godot.Collections;
using System.Linq;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;

[Tool]
public partial class BuildMenu : VBoxContainer
{
    [Export]
    public TowerState? TowerState { get; set; }

    [Export]
    public Label? NodeTowerName { get; set; }

    [Export]
    public Control? NodeWallet { get; set; }

    [Export]
    public Control? NodeRooms { get; set; }

    [Export]
    public Control? NodeFloors { get; set; }

    [Export]
    public Control? NodeTransports { get; set; }

    [Export]
    public RichTextLabel? WhatAreWeBuildingLabel { get; set; }

    public override void _Ready()
    {
        if (GlobalSignals.Singleton is GlobalSignals g)
        {
            g.OnTowerResourceChanged += _onTowerResourceChanged;
        }
        if (TowerState is not null)
            SetTower(TowerState);
    }

    public void SetTower(TowerState state)
    {
        TowerState = state;
        if (NodeWallet is not null)
            foreach (var (key, value) in TowerState.Wallet)
                _addItemLabelToWallet(NodeWallet, key, value);
        if (NodeTowerName is not null)
            NodeTowerName.Text = state.Name;
        if (NodeRooms is not null)
            _setRooms(NodeRooms, state.UnlockedRooms);
    }

    private void _onTowerResourceChanged(events.TowerResourceChangedEvent @event)
    {
        if (TowerState is null || NodeWallet is null || @event.Tower != TowerState) return;
        foreach (var key in @event.Amount.Keys)
        {
            var value = TowerState.Wallet[key];
            if (NodeWallet.ChildControl(key.Name) is not Control child)
                child = _addItemLabelToWallet(NodeWallet, key, value);
            // we do not remove labels that are zero or less
            child.Child<Label>()!.Text = value.ToString();
        }
    }

    private static HBoxContainer _addItemLabelToWallet(Control nodewallet, ItemDefinition item, uint amount) => nodewallet.AddedChild(
        new HBoxContainer { Name = item.Name }
            .WithChild(new Label { Text = $"{amount}" })
            .WithChild(new TextureRect { Texture = item.Icon })
    );

    private void _setRooms(Control nodeRooms, Array<RoomDefinition> rooms)
    {
        var names = rooms.ToDictionary(x => x.Name ?? "UNKNOWN", x => x).ToGodotDictionary();
        foreach (var child in nodeRooms.GetChildren())
        {
            if (!names.ContainsKey(child.Name))
                nodeRooms.RemoveChild(child);
            else
                names.Remove(child.Name);
        }

        // the remaining names that aren't already here
        foreach (var (name, def) in names)
        {
            nodeRooms.AddChild(
                new BuildButton() { Name = name, SizeFlagsHorizontal = SizeFlags.ExpandFill, RoomDefinition = def }
                .Configured(x =>
                {
                    x.OnClicked += _x_OnClicked;
                })
                .WithChild(new TextureRect { Texture = def.Icon, ExpandMode = TextureRect.ExpandModeEnum.FitWidth })
                .WithChild(new Label { Text = name, SizeFlagsHorizontal = SizeFlags.ExpandFill })
                .WithChild(new RichTextLabel { 
                    BbcodeEnabled = true, 
                    Text = _toStringAsCost(def.CostToBuildPerUnit), 
                    FitContent = true, 
                    ClipContents = false,
                    AutowrapMode = TextServer.AutowrapMode.Off,
                })
            );
        }
    }

    private string _toStringAsCost(NumericDict<ItemDefinition, uint>? cost)
    {
        if (cost is null || cost.Count == 0)
            return "Free";
        return string.Join(" + ", cost.Select(kv =>
        {
            var color = "white";
            uint walletAmount = 0;
            if (TowerState is null || !TowerState.Wallet.TryGetValue(kv.Key, out walletAmount) || walletAmount < kv.Value)
                color = "red";
            return $"[color={color}]{kv.Value}[/color][img height=24]{kv.Key.Icon?.ResourcePath}[/img]";
        }));
    }

    private void _x_OnClicked(BuildButton btn)
    {
        if (btn.RoomDefinition is not null && TowerState is not null)
            if (TowerState.Wallet >= btn.RoomDefinition.CostToBuildPerUnit && GlobalSignals.StartingRoomConstruction(new(TowerState, btn.RoomDefinition)).IsAllowed)
                GlobalSignals.StartedRoomConstruction(new(TowerState, btn.RoomDefinition));
    }

    public void SetWhatAreWeBuilding(Resource b)
    {
        if (WhatAreWeBuildingLabel is null)
            return;
        if (b is RoomDefinition room)
        {
            WhatAreWeBuildingLabel.Text = $"Building: [img height=24]{room.Icon?.ResourcePath}[/img] {room.Name}";
        }
    }
}
