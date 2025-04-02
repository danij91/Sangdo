using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFindingPlayer : MonoBehaviour
{
    public Tilemap walkableTilemap; // 이동 가능한 타일맵
    public Tilemap unwalkableTilemap; // 이동 불가능한 타일맵
    public float moveSpeed = 5f;  // 플레이어 이동 속도
    public float nextWaypointDistance = 0.1f; // 다음 경유지 도착 인정 거리

    private Vector2Int currentTile;  // 현재 타일의 좌표
    private List<Vector2Int> path = new List<Vector2Int>();  // 경로 리스트
    private int currentPathIndex = 0;  // 경로 리스트에서 이동할 위치
    private Coroutine moveCoroutine; // 이동 코루틴

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // 마우스 클릭 시
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int targetTile = (Vector2Int)walkableTilemap.WorldToCell(worldPosition);
            currentTile = (Vector2Int)walkableTilemap.WorldToCell(transform.position);

            // 새로운 목표 지점이 클릭되면 기존 이동 중단
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            // 새로운 경로 계산 및 이동 시작
            path = FindPathOptimized(currentTile, targetTile);
            currentPathIndex = 0;
            if (path.Count > 0)
            {
                moveCoroutine = StartCoroutine(MoveAlongPathCoroutine());
            }
        }
    }

    IEnumerator MoveAlongPathCoroutine()
    {
        while (currentPathIndex < path.Count)
        {
            Vector3 targetPosition = walkableTilemap.GetCellCenterWorld((Vector3Int)path[currentPathIndex]);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < nextWaypointDistance)
            {
                currentPathIndex++;
            }
            yield return null;
        }
        moveCoroutine = null; // 이동 완료
    }

    // 최적화된 A* 알고리즘을 사용해 경로 찾기 (대각선 이동 포함)
    public List<Vector2Int> FindPathOptimized(Vector2Int start, Vector2Int goal)
    {
        PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>();

        gScore[start] = 0;
        fScore[start] = GetHeuristicDistance(start, goal);
        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();

            if (current == goal)
            {
                return RetracePath(cameFrom, current);
            }

            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || !IsWalkable(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + GetDistance(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + GetHeuristicDistance(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                    else
                    {
                        openSet.UpdatePriority(neighbor, fScore[neighbor]); // 우선순위 큐 업데이트
                    }
                }
            }
        }

        return new List<Vector2Int>(); // 경로를 찾지 못함
    }

    // 휴리스틱 거리 계산 (대각선 이동 고려) - 맨하탄 거리 사용 (성능 향상 및 격자 맵에 적합)
    float GetHeuristicDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // 실제 이동 비용 계산 (대각선 이동 고려)
    float GetDistance(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        if (dx > dy)
            return 1f * dy + 1.414f * (dx - dy);
        else
            return 1f * dx + 1.414f * (dy - dx);
    }

    // 타일이 이동 가능한지 확인
    bool IsWalkable(Vector2Int position)
    {
        return walkableTilemap.HasTile((Vector3Int)position) && !unwalkableTilemap.HasTile((Vector3Int)position);
    }

    // 인접한 타일들을 반환 (대각선 이동 포함)
    List<Vector2Int> GetNeighbors(Vector2Int position)
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

    List<Vector2Int> RetracePath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
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

// 우선순위 큐 구현 (A* 알고리즘 성능 향상을 위해 사용)
public class PriorityQueue<T>
{
    private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
        elements.Sort((a, b) => a.Value.CompareTo(b.Value)); // 낮은 우선순위가 먼저
    }

    public T Dequeue()
    {
        if (elements.Count == 0)
        {
            throw new System.InvalidOperationException("PriorityQueue is empty");
        }
        T item = elements[0].Key;
        elements.RemoveAt(0);
        return item;
    }

    public bool Contains(T item)
    {
        return elements.Any(element => EqualityComparer<T>.Default.Equals(element.Key, item));
    }

    public void UpdatePriority(T item, float newPriority)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(elements[i].Key, item))
            {
                elements[i] = new KeyValuePair<T, float>(item, newPriority);
                elements.Sort((a, b) => a.Value.CompareTo(b.Value));
                return;
            }
        }
    }
}