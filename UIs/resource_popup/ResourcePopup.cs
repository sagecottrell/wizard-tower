using Godot;

namespace wizardtower.UIs.resource_popup;

[Tool]
[GlobalClass]
public partial class ResourcePopup : RichTextLabel
{
	double time = 0;
	Vector2 startPos;

	[Export(PropertyHint.ExpEasing)] public float Easing = 1;
	[Export] public float MaxTime = 0.5f;
	[Export] public float Distance = 25f;

	[Signal] public delegate void OnDoneEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startPos = Position;
		this.Log(Text);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		time += delta;
		Position = startPos + Vector2.Up * (float)Mathf.Ease(time / MaxTime, Easing) * Distance;
		if (time >= MaxTime)
			EmitSignalOnDone();
	}
}
