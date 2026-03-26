using Godot;
using wizardtower.events;

namespace wizardtower;

public partial class GlobalSignals : Node
{
    public static GlobalSignals? Singleton { get; private set; }

    // Use _EnterTree to make sure the Singleton instance is avaiable in _Ready()
    public override void _EnterTree()
    {
        if (Singleton != null && Singleton != this)
        {
            QueueFree(); // The Singleton is already loaded, kill this instance
        }
        Singleton = this;
    }

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnStartingRoomConstructionEventHandler(StartingRoomConstructionEvent @event);
    public static StartingRoomConstructionEvent StartingRoomConstruction(StartingRoomConstructionEvent @event) => _call(SignalName.OnStartingRoomConstruction, @event);

    [Signal]
    public delegate void OnStartedRoomConstructionEventHandler(StartedRoomConstructionEvent @event);
    public static StartedRoomConstructionEvent StartedRoomConstruction(StartedRoomConstructionEvent @event) => _call(SignalName.OnStartedRoomConstruction, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnStoppedRoomConstructionEventHandler(StoppedRoomConstructionEvent @event);
    public static void StoppedRoomConstruction(StoppedRoomConstructionEvent @event) => _call(SignalName.OnStoppedRoomConstruction, @event);

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
    public delegate void OnTowerResourceChangingEventHandler(TowerResourceChangingEvent @event);
    public static TowerResourceChangingEvent TowerResourceChanging(TowerResourceChangingEvent @event) => _call(SignalName.OnTowerResourceChanging, @event);

    [Signal]
    public delegate void OnTowerResourceChangedEventHandler(TowerResourceChangedEvent @event);
    public static TowerResourceChangedEvent TowerResourceChanged(TowerResourceChangedEvent @event) => _call(SignalName.OnTowerResourceChanged, @event);

    // ==================================================================================================================
    // ==================================================================================================================

    public static bool LogGlobalSignals { get; set; } = true;

    private static T _call<T>(string action, T ev) where T : GodotObject
    {
        Singleton?.EmitSignal(action, ev);
        if (LogGlobalSignals)
        {
            var p = ev is IDebug d ? d.DebugString(depth: 0) : typeof(T).Name;
            Singleton?.Log($"{action}|{p}");
        }
            
        return ev;
    }
}
