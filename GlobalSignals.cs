using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using wizardtower.events;
using wizardtower.events.ui;

namespace wizardtower;

[Tool]
public partial class GlobalSignals : Node
{
    private static GlobalSignals singleton;

    public static GlobalSignals Singleton {
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

    // Use _EnterTree to make sure the Singleton instance is available in _Ready()
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
    public static RoomConstructionSelectingEvent RoomConstructionSelecting(RoomConstructionSelectingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomConstructionSelecting, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnRoomConstructionSelectedEventHandler(RoomConstructionSelectedEvent @event);
    public static RoomConstructionSelectedEvent RoomConstructionSelected(RoomConstructionSelectedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomConstructionSelected, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnRoomConstructionStoppingEventHandler(RoomConstructionStoppingEvent @event);
    public static RoomConstructionStoppingEvent RoomConstructionStopping(RoomConstructionStoppingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomConstructionStopping, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnRoomConstructionStoppedEventHandler(RoomConstructionStoppedEvent @event);
    public static RoomConstructionStoppedEvent RoomConstructionStopped(RoomConstructionStoppedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomConstructionStopped, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnRoomConstructingEventHandler(RoomConstructingEvent @event);
    public static RoomConstructingEvent RoomConstructing(RoomConstructingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomConstructing, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnRoomConstructedEventHandler(RoomConstructedEvent @event);
    public static RoomConstructedEvent RoomConstructed(RoomConstructedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomConstructed, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnRoomDestroyingEventHandler(RoomDestroyingEvent @event);
    public static RoomDestroyingEvent RoomDestroying(RoomDestroyingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomDestroying, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnRoomDestroyedEventHandler(RoomDestroyedEvent @event);
    public static RoomDestroyedEvent RoomDestroyed(RoomDestroyedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomDestroyed, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnRoomConstructionPreviewEventHandler(RoomConstructionPreviewEvent @event);
    public static RoomConstructionPreviewEvent RoomConstructionPreview(RoomConstructionPreviewEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomConstructionPreview, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnRoomConstructionPreviewStoppedEventHandler(RoomConstructionPreviewStoppedEvent @event);
    public static RoomConstructionPreviewStoppedEvent RoomConstructionPreviewStopped(RoomConstructionPreviewStoppedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnRoomConstructionPreviewStopped, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorConstructionSelectingEventHandler(FloorConstructionSelectingEvent @event);
    public static FloorConstructionSelectingEvent FloorConstructionSelecting(FloorConstructionSelectingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorConstructionSelecting, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnFloorConstructionSelectedEventHandler(FloorConstructionSelectedEvent @event);
    public static FloorConstructionSelectedEvent FloorConstructionSelected(FloorConstructionSelectedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorConstructionSelected, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorConstructingEventHandler(FloorConstructingEvent @event);
    public static FloorConstructingEvent FloorConstructing(FloorConstructingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorConstructing, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnFloorConstructedEventHandler(FloorConstructedEvent @event);
    public static FloorConstructedEvent FloorConstructed(FloorConstructedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorConstructed, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorExtendingEventHandler(FloorExtendingEvent @event);
    public static FloorExtendingEvent FloorExtending(FloorExtendingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorExtending, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnFloorExtendedEventHandler(FloorExtendedEvent @event);
    public static FloorExtendedEvent FloorExtended(FloorExtendedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorExtended, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorReplacingEventHandler(FloorReplacingEvent @event);
    public static FloorReplacingEvent FloorReplacing(FloorReplacingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorReplacing, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnFloorReplacedEventHandler(FloorReplacedEvent @event);
    public static FloorReplacedEvent FloorReplaced(FloorReplacedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorReplaced, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnFloorConstructionStoppingEventHandler(FloorConstructionStoppingEvent @event);
    public static FloorConstructionStoppingEvent FloorConstructionStopping(FloorConstructionStoppingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorConstructionStopping, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnFloorConstructionStoppedEventHandler(FloorConstructionStoppedEvent @event);
    public static FloorConstructionStoppedEvent FloorConstructionStopped(FloorConstructionStoppedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnFloorConstructionStopped, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnTransportConstructionSelectingEventHandler(TransportConstructionSelectingEvent @event);
    public static TransportConstructionSelectingEvent TransportConstructionSelecting(TransportConstructionSelectingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportConstructionSelecting, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnTransportConstructionSelectedEventHandler(TransportConstructionSelectedEvent @event);
    public static TransportConstructionSelectedEvent TransportConstructionSelected(TransportConstructionSelectedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportConstructionSelected, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnTransportConstructionStoppingEventHandler(TransportConstructionStoppingEvent @event);
    public static TransportConstructionStoppingEvent TransportConstructionStopping(TransportConstructionStoppingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportConstructionStopping, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnTransportConstructionStoppedEventHandler(TransportConstructionStoppedEvent @event);
    public static TransportConstructionStoppedEvent TransportConstructionStopped(TransportConstructionStoppedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportConstructionStopped, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnTransportConstructingEventHandler(TransportConstructingEvent @event);
    public static TransportConstructingEvent TransportConstructing(TransportConstructingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportConstructing, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnTransportConstructedEventHandler(TransportConstructedEvent @event);
    public static TransportConstructedEvent TransportConstructed(TransportConstructedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportConstructed, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnTransportDestroyingEventHandler(TransportDestroyingEvent @event);
    public static TransportDestroyingEvent TransportDestroying(TransportDestroyingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportDestroying, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnTransportDestroyedEventHandler(TransportDestroyedEvent @event);
    public static TransportDestroyedEvent TransportDestroyed(TransportDestroyedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportDestroyed, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnTransportConstructionPreviewEventHandler(TransportConstructionPreviewEvent @event);
    public static TransportConstructionPreviewEvent TransportConstructionPreview(TransportConstructionPreviewEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportConstructionPreview, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnTransportConstructionPreviewStoppedEventHandler(TransportConstructionPreviewStoppedEvent @event);
    public static TransportConstructionPreviewStoppedEvent TransportConstructionPreviewStopped(TransportConstructionPreviewStoppedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTransportConstructionPreviewStopped, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnTowerResourceChangingEventHandler(TowerResourceChangingEvent @event);
    public static TowerResourceChangingEvent TowerResourceChanging(TowerResourceChangingEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTowerResourceChanging, @event, callerFile, callerLine);

    [Signal]
    public delegate void OnTowerResourceChangedEventHandler(TowerResourceChangedEvent @event);
    public static TowerResourceChangedEvent TowerResourceChanged(TowerResourceChangedEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnTowerResourceChanged, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    [Signal]
    public delegate void OnCancelledUIEventHandler(CancelledUIEvent @event);
    public static CancelledUIEvent CancelledUI(CancelledUIEvent @event, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0) => _call(SignalName.OnCancelledUI, @event, callerFile, callerLine);

    // ==================================================================================================================
    // ==================================================================================================================

    private const string indenter = "| ";
    private static int indent = 0;
    private static readonly List<string> indents = [];

    private static T _call<T>(string action, T ev, string callerFile = "", int callerLine = 0) where T : GodotObject
    {
        indent++;
        Singleton?.EmitSignal(action, ev);
        indent--;
        if (Input.IsActionPressed(InputMapConstants.LogGlobalSignals))
        {
            var p = ev is IDebug d ? d.DebugString(depth: 0) : typeof(T).Name;
            var location = string.IsNullOrEmpty(callerFile) ? "" : $"{Path.GetFileName(callerFile)}:{callerLine}";

            if (indents.Count < indent)
                indents.AddRange(Enumerable.Repeat(indenter, indent - indents.Count));
            Singleton?.Debug($"{location}|{action}|{p}".Indent(string.Join("", indents[..indent])));
        }
        
        return ev;
    }
}
