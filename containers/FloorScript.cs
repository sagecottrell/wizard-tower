using Godot;
using System.Collections.Generic;
using System.Linq;
using wizardtower.events;
using wizardtower.events.interfaces;
using wizardtower.events.ui;
using wizardtower.state;

namespace wizardtower.containers;

[Tool]
[GlobalClass]
public partial class FloorScript(TowerState towerState, FloorState floorState) : Node3D()
{
    [ExportToolButton("OnCreate")]
    public Callable TriggerOnCreate => Callable.From(OnCreate);

    public TowerState TowerState { get; } = towerState;
    public FloorState FloorState { get; } = floorState;

    private readonly Dictionary<int, FloorBackgroundTileScript> _tiles = [];

    public override void _Ready()
    {
        Name = $"Floor{FloorState.Elevation}";
        Position = this.TowerCoordToNodePosition(y: FloorState.Elevation);
    }

    public override void _EnterTree()
    {
        GlobalSignals.Singleton.OnRoomConstructionPreview += _g_OnRoomConstructionPreview;
        GlobalSignals.Singleton.OnRoomConstructed += _g_OnRoomConstructed;
        GlobalSignals.Singleton.OnRoomDestroyed += _g_OnRoomDestroyed;
        GlobalSignals.Singleton.OnFloorExtended += _g_OnFloorExtended;
        GlobalSignals.Singleton.OnFloorReplaced += _g_OnFloorReplaced;
        GlobalSignals.Singleton.OnRoomConstructionPreviewStopped += _g_OnConstructionPreviewStopped;
        GlobalSignals.Singleton.OnTransportConstructionPreviewStopped += _g_OnConstructionPreviewStopped;
    }

    public override void _ExitTree()
    {
        GlobalSignals.Singleton.OnRoomConstructionPreview -= _g_OnRoomConstructionPreview;
        GlobalSignals.Singleton.OnRoomConstructed -= _g_OnRoomConstructed;
        GlobalSignals.Singleton.OnRoomDestroyed -= _g_OnRoomDestroyed;
        GlobalSignals.Singleton.OnFloorExtended -= _g_OnFloorExtended;
        GlobalSignals.Singleton.OnFloorReplaced -= _g_OnFloorReplaced;
        GlobalSignals.Singleton.OnRoomConstructionPreviewStopped -= _g_OnConstructionPreviewStopped;
        GlobalSignals.Singleton.OnTransportConstructionPreviewStopped -= _g_OnConstructionPreviewStopped;
    }


    private void _g_OnConstructionPreviewStopped(IEvent @event)
    {
        MakeAllVisible();
    }

    private void _g_OnFloorReplaced(FloorReplacedEvent @event)
    {
        if (@event.Floor.Elevation != FloorState.Elevation)
            return;
        foreach (var (id, tile) in _tiles.ToList())
        {
            _removeTile(id);
            var r = _addTile(id);
            r?.WallVisible(tile.Wall?.Visible ?? true);
        }
    }

    private void _g_OnFloorExtended(FloorExtendedEvent @event)
    {
        if (@event.Floor.Elevation != FloorState.Elevation)
            return;
        SetPositionVisible(@event.Floor.LeftBound, @event.ExtendedLeft, true);
        SetPositionVisible(@event.Floor.RightBound - (int)@event.ExtendedRight + 1, @event.ExtendedRight, true);
    }

    private void _g_OnRoomConstructed(RoomConstructedEvent @event)
    {
        if (@event.Room.Elevation != FloorState.Elevation)
            return;
        SetPositionVisible(@event.Room, false);
    }

    private void _g_OnRoomDestroyed(RoomDestroyedEvent @event)
    {
        if (@event.Room.Elevation != FloorState.Elevation)
            return;
        SetPositionVisible(@event.Room, true);
    }

    private void _g_OnRoomConstructionPreview(RoomConstructionPreviewEvent @event)
    {
        MakeAllVisible();
        if (@event.Room.Elevation == FloorState.Elevation)
        {
            SetPositionVisible(@event.Room.FloorPosition, @event.Room.Definition.Width, false);
        }
    }

    public void SetupTiles()
    {
        foreach (var id in _tiles.Keys)
        {
            if (id < FloorState.LeftBound || id > FloorState.RightBound)
                _removeTile(id);
        }

        for (var i = FloorState.LeftBound; i <= FloorState.RightBound; i++)
        {
            _addTile(i);
        }
    }

    private void _removeTile(int i)
    {
        if (_tiles.Remove(i, out var tile))
        {
            // TODO: peform animation before removing child
            RemoveChild(tile);
            tile.QueueFree();
        }
    }

    private FloorBackgroundTileScript? _addTile(int i)
    {
        if (FloorState.Definition.FloorBackgroundTileScene is not PackedScene scene)
            return null;
        if (_tiles.TryGetValue(i, out var tile))
            return tile;
        if (scene.Instantiate() is FloorBackgroundTileScript fbt && _tiles.TryAdd(i, fbt))
        {
            AddChild(fbt);
            fbt.Index = i;
            fbt.Position = fbt.TowerCoordToNodePosition(x: i);
            fbt.InDirection = -Mathf.Sign(FloorState.Elevation);
            fbt.OnCreate();

            if (!TowerState.PositionVacant(FloorState.Elevation, i))
                fbt.WallVisible(false);
            return fbt;
        }
        return null;
    }

    public void SetPositionVisible(int left, uint width, bool visible)
    {
        for (var i = 0; i < width; i++)
        {
            _addTile(i + left)?.WallVisible(visible);
        }
    }

    public void SetPositionVisible(RoomState room, bool visible)
    {
        SetPositionVisible(room.FloorPosition, room.Definition.Width, visible);
    }

    public void MakeAllVisible()
    {
        foreach (var tile in _tiles.Values)
            tile.WallVisible(TowerState.PositionVacant(FloorState.Elevation, tile.Index));
    }

    public void OnCreate()
    {
        Position = this.TowerCoordToNodePosition(y: FloorState.Elevation);
    }

    public void Destroy()
    {
        QueueFree();
    }
}
