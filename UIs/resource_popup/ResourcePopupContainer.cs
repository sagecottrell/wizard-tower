using Godot;
using wizardtower;
using wizardtower.events.handlers;
using wizardtower.events.Tower;
using wizardtower.UIs.resource_popup;


public partial class ResourcePopupContainer : Control
{
	[Export(PropertyHint.ExpEasing)] public float Easing = 1;
	[Export] public float MaxTime = 0.5f;
	[Export] public float Distance = 25f;

	public override void _EnterTree() {
		TowerEvents.ResourceChanged += _onTowerResourceChanged;
	}

	public override void _ExitTree() {
		TowerEvents.ResourceChanged -= _onTowerResourceChanged;
	}

	private void _onTowerResourceChanged(TowerResourceChangedEvent @event) {
		if (@event.Input?.GodotInput is InputEventMouseButton mb) {
			this.AddedChild(new ResourcePopup() {
				Position = mb.Position,
				Text = $"[color=red]-{@event.Amount.ToStringAsCost(" - ")}[/color]",
				BbcodeEnabled = true,
				FitContent = true,
				AutowrapMode = 0,
				OffsetTransformEnabled = true,
				OffsetTransformPositionRatio = -Vector2.One / 2f,
				Easing = Easing,
				MaxTime = MaxTime,
				Distance = Distance,
			}.Configured(p => {
				p.OnDone += p.QueueFree;
			})); 
		}
	}
}
