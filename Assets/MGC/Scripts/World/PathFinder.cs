// ✅ PathFinder.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private float maxSearchDistance = 20f;
    private Tilemap walkableTilemap;
    private Tilemap unwalkableTilemap;

    public void SetTilemaps(Tilemap walkable, Tilemap unwalkable)
    {
        walkableTilemap = walkable;
        unwalkableTilemap = unwalkable;
    }

    public List<Vector2Int> FindPathOptimized(Vector2Int start, Vector2Int goal)
    {
        PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>();

        Vector2Int closestNode = start;
        float closestDistanceToGoal = GetHeuristicDistance(start, goal);

        gScore[start] = 0;
        fScore[start] = closestDistanceToGoal;
        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();

            if (gScore[current] > maxSearchDistance)
            {
                return RetracePath(cameFrom, closestNode);
            }

            if (current == goal)
            {
                return RetracePath(cameFrom, current);
            }

            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || !IsWalkable(neighbor)) continue;

                float tentativeGScore = gScore[current] + GetDistance(current, neighbor);
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + GetHeuristicDistance(neighbor, goal);

                    float distToGoal = GetHeuristicDistance(neighbor, goal);
                    if (distToGoal < closestDistanceToGoal)
                    {
                        closestDistanceToGoal = distToGoal;
                        closestNode = neighbor;
                    }

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    else
                        openSet.UpdatePriority(neighbor, fScore[neighbor]);
                }
            }
        }

        return RetracePath(cameFrom, closestNode);
    }

    private float GetHeuristicDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private float GetDistance(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return dx > dy ? 1f * dy + 1.414f * (dx - dy) : 1f * dx + 1.414f * (dy - dx);
    }

    private bool IsWalkable(Vector2Int position)
    {
        return walkableTilemap.HasTile((Vector3Int)position) &&
               !unwalkableTilemap.HasTile((Vector3Int)position);
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector2Int neighborPos = new Vector2Int(position.x + x, position.y + y);
                if (IsWalkable(neighborPos))
                {
                    neighbors.Add(neighborPos);
                }
            }
        }
        return neighbors;
    }

    private List<Vector2Int> RetracePath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }
}