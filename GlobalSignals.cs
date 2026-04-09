using Godot;
using wizardtower.events;

namespace wizardtower;

[Tool]
public partial class GlobalSignals : Node
{
    private static GlobalSignals? singleton;

    public static GlobalSignals? Singleton {
        get
        {
            if (Engine.IsEditorHint())
            {
                var root = EditorInterface.Singleton.GetEditedSceneRoot();
                if (root.Child<GlobalSignals>() is not GlobalSignals gs)
                {
                    gs = new GlobalSignals();
                    root.AddChild(gs);
                }
                return gs;
            }
            return singleton;
        }
        private set => singleton = value; 
    }

    // Use _EnterTree to make sure the Singleton instance is avaiable in _Ready()
    public override void _EnterTree()
    {
        if (Engine.IsEditorHint())
            return;
        if (Singleton != null && Singleton != this)
        {
            QueueFree(); // The Singleton is already loaded, kill this instance
        }
        Singleton = this;
    }

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnRoomConstructionSelectingEventHandler(RoomConstructionSelectingEvent @event);
    public static RoomConstructionSelectingEvent RoomConstructionSelecting(RoomConstructionSelectingEvent @event) => _call(SignalName.OnRoomConstructionSelecting, @event);

    [Signal]
    public delegate void OnRoomConstructionSelectedEventHandler(RoomConstructionSelectedEvent @event);
    public static RoomConstructionSelectedEvent RoomConstructionSelected(RoomConstructionSelectedEvent @event) => _call(SignalName.OnRoomConstructionSelected, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnRoomConstructionStoppingEventHandler(RoomConstructionStoppingEvent @event);
    public static RoomConstructionStoppingEvent RoomConstructionStopping(RoomConstructionStoppingEvent @event) => _call(SignalName.OnRoomConstructionStopping, @event);

    [Signal]
    public delegate void OnRoomConstructionStoppedEventHandler(RoomConstructionStoppedEvent @event);
    public static RoomConstructionStoppedEvent RoomConstructionStopped(RoomConstructionStoppedEvent @event) => _call(SignalName.OnRoomConstructionStopped, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnRoomConstructingEventHandler(RoomConstructingEvent @event);
    public static RoomConstructingEvent RoomConstructing(RoomConstructingEvent @event) => _call(SignalName.OnRoomConstructing, @event);

    [Signal]
    public delegate void OnRoomConstructedEventHandler(RoomConstructedEvent @event);
    public static RoomConstructedEvent RoomConstructed(RoomConstructedEvent @event) => _call(SignalName.OnRoomConstructed, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnRoomConstructionPreviewEventHandler(RoomConstructionPreviewEvent @event);
    public static RoomConstructionPreviewEvent RoomConstructionPreview(RoomConstructionPreviewEvent @event) => _call(SignalName.OnRoomConstructionPreview, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorConstructionSelectingEventHandler(FloorConstructionSelectingEvent @event);
    public static FloorConstructionSelectingEvent FloorConstructionSelecting(FloorConstructionSelectingEvent @event) => _call(SignalName.OnFloorConstructionSelecting, @event);

    [Signal]
    public delegate void OnFloorConstructionSelectedEventHandler(FloorConstructionSelectedEvent @event);
    public static FloorConstructionSelectedEvent FloorConstructionSelected(FloorConstructionSelectedEvent @event) => _call(SignalName.OnFloorConstructionSelected, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorConstructingEventHandler(FloorConstructingEvent @event);
    public static FloorConstructingEvent FloorConstructing(FloorConstructingEvent @event) => _call(SignalName.OnFloorConstructing, @event);

    [Signal]
    public delegate void OnFloorConstructedEventHandler(FloorConstructedEvent @event);
    public static FloorConstructedEvent FloorConstructed(FloorConstructedEvent @event) => _call(SignalName.OnFloorConstructed, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorExtendingEventHandler(FloorExtendingEvent @event);
    public static FloorExtendingEvent FloorExtending(FloorExtendingEvent @event) => _call(SignalName.OnFloorExtending, @event);

    [Signal]
    public delegate void OnFloorExtendedEventHandler(FloorExtendedEvent @event);
    public static FloorExtendedEvent FloorExtended(FloorExtendedEvent @event) => _call(SignalName.OnFloorExtended, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorReplacingEventHandler(FloorReplacingEvent @event);
    public static FloorReplacingEvent FloorReplacing(FloorReplacingEvent @event) => _call(SignalName.OnFloorReplacing, @event);

    [Signal]
    public delegate void OnFloorReplacedEventHandler(FloorReplacedEvent @event);
    public static FloorReplacedEvent FloorReplaced(FloorReplacedEvent @event) => _call(SignalName.OnFloorReplaced, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorConstructionStoppingEventHandler(FloorConstructionStoppingEvent @event);
    public static FloorConstructionStoppingEvent FloorConstructionStopping(FloorConstructionStoppingEvent @event) => _call(SignalName.OnFloorConstructionStopping, @event);

    [Signal]
    public delegate void OnFloorConstructionStoppedEventHandler(FloorConstructionStoppedEvent @event);
    public static FloorConstructionStoppedEvent FloorConstructionStopped(FloorConstructionStoppedEvent @event) => _call(SignalName.OnFloorConstructionStopped, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnTowerResourceChangingEventHandler(TowerResourceChangingEvent @event);
    public static TowerResourceChangingEvent TowerResourceChanging(TowerResourceChangingEvent @event) => _call(SignalName.OnTowerResourceChanging, @event);

    [Signal]
    public delegate void OnTowerResourceChangedEventHandler(TowerResourceChangedEvent @event);
    public static TowerResourceChangedEvent TowerResourceChanged(TowerResourceChangedEvent @event) => _call(SignalName.OnTowerResourceChanged, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    private static T _call<T>(string action, T ev) where T : GodotObject
    {
        Singleton?.EmitSignal(action, ev);
        if (Input.IsActionPressed(InputMapConstants.LogGlobalSignals))
        {
            var p = ev is IDebug d ? d.DebugString(depth: 0) : typeof(T).Name;
            Singleton?.Debug($"{action}|{p}");
        }
        
        return ev;
    }
}
