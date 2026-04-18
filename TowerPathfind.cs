using System;
using System.Collections.Generic;
using wizardtower.state;

namespace wizardtower;

public static class TowerPathfind
{
    public static PathfindNode? Pathfind(TowerState tower, RoomState from, RoomState to, uint limit)
    {
        // the priority is the distance from the start room. meaning we are implementing a breadth first search.
        PriorityQueue<PathfindNode, int> queue = new();
        queue.Enqueue(new(from.Elevation, from.FloorPosition, 0), 0);

        while (queue.Count > 0)
        {
            if (!queue.TryDequeue(out var current, out var priority))
                break;
            var (elevation, position, count, _) = current;
            if (elevation == to.Elevation && position == to.FloorPosition)
                return current;

            void tryEnqueue(int elevation, int position, int extraPriority)
            {
                // we don't need to enqueue if it's past the limit, because we won't be able to reach the destination in time. this is a simple optimization to reduce the search space.
                if (count < limit)
                    queue.Enqueue(new(elevation, position, count + 1, current), priority + extraPriority);
            }

            foreach (var transport in tower.TransportsOnFloor(elevation))
            {
                var dh = Math.Abs(transport.HorizontalPosition - position);
                var stopsAlongTheWay = 0;
                for (var i = elevation; i >= transport.Elevation; i--)
                {
                    if (transport.Definition.CanStopAtFloor.Contains(tower.Floors[i].Definition))
                        tryEnqueue(i, transport.HorizontalPosition, stopsAlongTheWay + dh);
                }

                stopsAlongTheWay = 0;
                for (var i = elevation; i <= transport.Elevation + transport.Height; i++)
                {
                    if (transport.Definition.CanStopAtFloor.Contains(tower.Floors[i].Definition))
                        tryEnqueue(i, transport.HorizontalPosition, stopsAlongTheWay + dh);
                }
            }
            foreach (var room in tower.RoomsOnFloor(elevation))
            {
                if (room == to)
                    tryEnqueue(elevation, room.FloorPosition, Math.Abs(room.FloorPosition - position));
                // in the future, teleporting rooms might also go here
            }
        }
        return null;
    }
}


public record class PathfindNode(int Elevation, int FloorPosition, int Count, PathfindNode? Previous = null)
{
    public List<(int Elevation, int FloorPosition)> GetPath()
    {
        List<(int Elevation, int FloorPosition)> path = [];
        var current = this;
        while (current != null)
        {
            path.Add((current.Elevation, current.FloorPosition));
            current = current.Previous;
        }
        path.Reverse();
        return path;
    }
}

