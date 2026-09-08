using System.Collections.Generic;
using System.Linq;
using Godot;
using wizardtower.actions;
using wizardtower.actions.ui;
using wizardtower.containers;
using wizardtower.events.handlers;
using wizardtower.events.Interface;
using wizardtower.events.interfaces;
using wizardtower.events.Room.ui;
using wizardtower.state;
using wizardtower.UIs.room_details;
using wizardtower.UIs.selector;

namespace wizardtower.UIs.configure_worker_paths;

/// <summary>
/// <para>
/// - user initiates add worker path via RoomDeliveryPlanningStartedEvent
/// </para>
/// <para>
/// - show partial worker path
/// </para>
/// <para>
/// - emit RoomDeliveryPlanningQueryEvent, display Selectors from NextValidPositions
/// </para>
/// </summary>
public partial class AddWorkerPathOverlay(TowerScript tower) : Node3D(), IUserInterface
{
    readonly List<TransportToTake> _currentPaths = [];
    Vector3? _lastpos = null;
    RoomDeliveryPlanningStartedEvent? _lastEvent;

    public override void _EnterTree()
    {
        RoomEvents.Ui.DeliveryPlanningStarted += _onDeliveryPlanningStarted;
        RoomEvents.Ui.DeliveryPlanningCompleted += _onDeliveryPlanningCompleted;
        RoomEvents.Ui.Selecting += _denyIfActive;
        TransportEvents.Ui.Selecting += _denyIfActive;
        InterfaceEvents.Hided += _onHided;
    }

    public override void _ExitTree()
    {
        RoomEvents.Ui.DeliveryPlanningStarted -= _onDeliveryPlanningStarted;
        RoomEvents.Ui.DeliveryPlanningCompleted -= _onDeliveryPlanningCompleted;
        RoomEvents.Ui.Selecting -= _denyIfActive;
        TransportEvents.Ui.Selecting -= _denyIfActive;
        InterfaceEvents.Hided -= _onHided;
    }

    void _reset_ui()
    {
        this.FreeChildren<ResourceDeliveryVisualizer>();
        this.FreeChildren<Selector>();
    }

    void _stopping()
    {
        _lastpos = null;
    }

    public bool IsActive() => _lastpos is not null;

    void _onDeliveryPlanningCompleted(RoomDeliveryPlanningCompletedEvent ev)
    {
        _reset_ui();
        _stopping();
    }

    void _onHided(InterfaceHidedEvent ev)
    {
        _reset_ui();
        _stopping();
    }

    void _denyIfActive(IDeniableEvent ev)
    {
        if (IsActive())
            ev.IsAllowed = false;
    }

    void _onDeliveryPlanningStarted(RoomDeliveryPlanningStartedEvent @event)
    {
        if (@event.TowerState != tower.State)
            return;
        if (@event.ItemDefinitions.Count == 0)
        {
            _reset_ui();
            return;
        }
        if (@event.RoomState != _lastEvent?.RoomState)
            _reset_ui();

        _lastEvent = @event;
        _currentPaths.Clear();
        _lastpos = null;

        _query();
    }

    void _query()
    {
        if (_lastEvent?.RoomState is not { } currentRoom)
            return;

        var path = new RoomStateWorkerPath()
        {
            ItemDefinition = null!,
            TransportsToTake = [.. _currentPaths],
        };
        var queryEvent = new RoomDeliveryPlanningQueryEvent(tower.State, currentRoom, _lastEvent.ItemDefinitions, path);

        _reset_ui();

        _showDeliveryVisualizers();

        RoomEvents.Ui.OnDeliveryPlanningQuery(queryEvent);

        _showSelectors(queryEvent);
    }

    void _showSelectors(RoomDeliveryPlanningQueryEvent queryEvent)
    {
        foreach (var pos in queryEvent.NextValidPositions)
        {
            if (SceneLoader.TryLoadScene<Selector>(out var selector))
            {
                selector.IncreaseRight = pos.Width - 1;
                selector.IncreaseUp = pos.Height - 1;
                selector.Position = pos.ToVector3();
                selector.OnAccept += ue => _selectorAccept(ue, pos);
                AddChild(selector);
            }
        }
    }

    void _showDeliveryVisualizers()
    {
        if (_lastEvent?.RoomState is not { } currentRoom || _currentPaths.Count == 0 || _lastpos is not { } lastpos)
            return;

        var offset = 0f;
        foreach (var itemDef in _lastEvent.ItemDefinitions)
        {
            var workerPath = new PartialRoomStateWorkerPath()
            {
                ItemDefinition = itemDef,
                TransportsToTake = [.. _currentPaths],
                FinalPosition = lastpos,
            };
            var vis = new ResourceDeliveryVisualizer()
            {
                WorkerPath = workerPath,
                FromRoomId = currentRoom.Id,
                TowerState = tower.State,
                Speed = 0.5f,
                ItemDistance = 2f,
                ItemScale = new(0.5f, 0.5f),
                Easing = 0.147f,
                TimeOffset = offset,
            };
            offset++;
            vis.SetupPath();
            vis.Position = currentRoom.Vec3Position(offset: new(0, 0.5f, 2));
            AddChild(vis);
        }
    }

    void _selectorAccept(UserEvent ue, RoomDeliveryPlanningQueryEvent.ValidPosition pos)
    {
        if (_lastEvent?.RoomState is not { } currentRoom || _lastEvent?.ItemDefinitions is not { } items)
            return;

        if (pos.Kind is RoomDeliveryPlanningQueryEvent.ValidPosition.RoomKind rk)
        {
            // If the selector is on a room, then we have reached a destination and we can stop the planning process.
            List<RoomStateWorkerPath> paths = [..items.Select(def => new RoomStateWorkerPath()
            {
                ItemDefinition = def,
                TargetRoomId = rk.State.Id,
                TransportsToTake = [.._currentPaths],
            })];

            RoomActions.AddDeliveryPlanningComplete(new(tower.State, currentRoom, paths) { Source = ue });
        }
        else if (pos.Kind is RoomDeliveryPlanningQueryEvent.ValidPosition.TransportKind tk)
        {
            // transports cannot be destinations
            _currentPaths.Add(new TransportToTake()
            {
                Elevation = pos.Elevation,
                TransportId = tk.State.Id,
            });
            _lastpos = new(tk.State.HorizontalPosition, pos.Elevation, 0);
            _query();
        }
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (IsActive() && @event.IsActionPressed(InputMapConstants.Cancel))
        {
            UIActions.Hide(new(this) { Source = new UserEvent(@event) });
        }
    }
}
