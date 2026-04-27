using Godot;
using System.Linq;
using wizardtower.events;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.tower;

public partial class WalletUI(TowerState towerState) : Node, IUserInterface
{
    private VBoxContainer ui = new();

    public override void _Ready()
    {
        AddChild(new PanelContainer().WithChild(ui));

        foreach (var (key, value) in towerState.Wallet)
            _addItemLabelToWallet(key, value);
    }

    public override void _EnterTree()
    {
        GlobalSignals.Singleton.OnTowerResourceChanged += _onTowerResourceChanged;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnTowerResourceChanged -= _onTowerResourceChanged;
    }

    private void _onTowerResourceChanged(TowerResourceChangedEvent @event)
    {
        if (towerState is null || @event.TowerState != towerState) return;
        foreach (var key in @event.Amount.Keys.OrderBy(x => x.Name))
        {
            var value = towerState.Wallet[key];
            if (ui.ChildControl(key.Name) is not Control child)
                child = _addItemLabelToWallet(key, value);
            // only remove labels that are zero or less if PersistInWallet is false
            if (value <= 0 && !key.PersistInWallet)
                child.QueueFree();
            child.Child<Label>()!.Text = value.ToString();
        }
    }

    private HBoxContainer _addItemLabelToWallet(ItemDefinition item, uint amount) => ui.AddedChild(
        new HBoxContainer { Name = item.Name, SizeFlagsHorizontal = Control.SizeFlags.ExpandFill, SizeFlagsVertical = Control.SizeFlags.ExpandFill, CustomMinimumSize = new(100, 0) }
            .WithChild(new TextureRect { Texture = item.Icon, TooltipText = item.Name, ExpandMode = TextureRect.ExpandModeEnum.FitWidth })
            .WithChild(new Label { Text = $"{amount}" })
    );
}
