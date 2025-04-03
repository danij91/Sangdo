using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap walkableTilemap; // 이동 가능한 타일맵
    public Tilemap unwalkableTilemap; // 이동 불가능한 타일맵
    public float moveSpeed = 5f; // 플레이어 이동 속도
    public float nextWaypointDistance = 0.1f; // 다음 경유지 도착 인정 거리

    [SerializeField] private float interactionRange = 0.7f;
    [SerializeField] private float maxSearchDistance = 20f;

    private Vector2Int currentTile; // 현재 타일의 좌표
    private List<Vector2Int> path = new(); // 경로 리스트
    private int currentPathIndex = 0; // 경로 리스트에서 이동할 위치
    private Coroutine moveCoroutine; // 이동 코루틴
    private Transform target;
    private IInteractable currentInteractionTarget;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = walkableTilemap.WorldToCell(worldPosition);
            Vector2Int targetTile = (Vector2Int)cellPosition;
            currentTile = (Vector2Int)walkableTilemap.WorldToCell(transform.position);

            // 클릭한 위치에 Interactable 오브젝트가 있는지 확인
            Collider2D hit = Physics2D.OverlapPoint(worldPosition);
            currentInteractionTarget = null;

            if (hit != null)
            {
                var interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    currentInteractionTarget = interactable;
                    currentInteractionTarget.OnSelect();
                    // 목표 타일을 해당 오브젝트 위치로 덮어쓰기 (정확한 타일로 이동)
                    targetTile = (Vector2Int)walkableTilemap.WorldToCell(hit.transform.position);
                }
            }

            // 이동 중이면 중단
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

    private IEnumerator MoveAlongPathCoroutine()
    {
        while (currentPathIndex < path.Count)
        {
            Vector3 targetPosition = walkableTilemap.GetCellCenterWorld((Vector3Int)path[currentPathIndex]);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 타겟 오브젝트가 있다면 일정 거리 이내 도착 체크
            if (currentInteractionTarget != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position,
                    ((MonoBehaviour)currentInteractionTarget).transform.position);
                if (distanceToTarget < interactionRange)
                {
                    break; // 경로 중단 → 상호작용
                }
            }

            if (Vector3.Distance(transform.position, targetPosition) < nextWaypointDistance)
            {
                currentPathIndex++;
            }

            yield return null;
        }

        // 도착 시 상호작용
        if (currentInteractionTarget != null)
        {
            currentInteractionTarget.Interact();
            currentInteractionTarget = null;
        }

        moveCoroutine = null;
    }

    // 최적화된 A* 알고리즘을 사용해 경로 찾기 (대각선 이동 포함)
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

            // ✅ 탐색 거리 제한
            if (gScore[current] > maxSearchDistance)
            {
                Debug.Log($"[Pathfinding] 탐색 거리 초과, {closestNode} 까지만 이동");
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

                    // ✅ 목표와 가장 가까운 점 추적
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

        // ✅ 목표에 도달하지 못했을 경우: 가장 가까운 지점까지의 경로 반환
        Debug.Log($"[Pathfinding] 목표 도달 실패. 최단 거리 노드({closestNode})로 이동");
        return RetracePath(cameFrom, closestNode);
    }


    // 휴리스틱 거리 계산 (대각선 이동 고려) - 맨하탄 거리 사용 (성능 향상 및 격자 맵에 적합)
    private float GetHeuristicDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // 실제 이동 비용 계산 (대각선 이동 고려)
    private float GetDistance(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        if (dx > dy)
            return 1f * dy + 1.414f * (dx - dy);
        else
            return 1f * dx + 1.414f * (dy - dx);
    }

    // 타일이 이동 가능한지 확인
    private bool IsWalkable(Vector2Int position)
    {
        return walkableTilemap.HasTile((Vector3Int)position) && !unwalkableTilemap.HasTile((Vector3Int)position);
    }

    // 인접한 타일들을 반환 (대각선 이동 포함)
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

    public void SetWalkableTilemap(Tilemap walkableTilemap)
    {
        this.walkableTilemap = walkableTilemap;
    }

    public void SetUnwalkableTilemap(Tilemap unwalkableTilemap)
    {
        this.unwalkableTilemap = unwalkableTilemap;
    }
}