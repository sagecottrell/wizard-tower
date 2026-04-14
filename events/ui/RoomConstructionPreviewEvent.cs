using Godot;
using wizardtower.events.interfaces;
using wizardtower.state;

namespace wizardtower.events.ui;

public partial class RoomConstructionPreviewEvent(TowerState towerState, RoomState? previewState) : GodotObject, IEvent, IDebug, ITowerEvent
{
    public TowerState TowerState { get; } = towerState;

    // if null, then no preview is being shown
    public RoomState? PreviewState { get; } = previewState;
}
