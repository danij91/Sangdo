using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float nextWaypointDistance = 0.1f;
    [SerializeField] private float interactionRange = 0.7f;

    private List<Vector2Int> path = new();
    private int currentPathIndex = 0;
    private Coroutine moveCoroutine;
    private IInteractable currentInteractionTarget;

    private Tilemap walkableTilemap;
    private PathFinder pathFinder;

    private void Awake()
    {
        pathFinder = GetComponent<PathFinder>();
    }

    public void SetWalkableTilemap(Tilemap tilemap)
    {
        walkableTilemap = tilemap;
    }

    public void MoveTo(Vector2Int currentTile, Vector2Int targetTile, IInteractable target = null)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        path = pathFinder.FindPathOptimized(currentTile, targetTile);
        currentPathIndex = 0;
        currentInteractionTarget = target;

        if (path.Count > 0)
        {
            moveCoroutine = StartCoroutine(MoveAlongPathCoroutine());
        }
    }

    private IEnumerator MoveAlongPathCoroutine()
    {
        while (currentPathIndex < path.Count)
        {
            Vector3 targetPosition = walkableTilemap.GetCellCenterWorld((Vector3Int)path[currentPathIndex]);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (currentInteractionTarget != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position,
                    ((MonoBehaviour)currentInteractionTarget).transform.position);
                if (distanceToTarget < interactionRange)
                {
                    break;
                }
            }

            if (Vector3.Distance(transform.position, targetPosition) < nextWaypointDistance)
            {
                currentPathIndex++;
            }

            yield return null;
        }

        if (currentInteractionTarget != null)
        {
            currentInteractionTarget.Interact();
            currentInteractionTarget = null;
        }

        moveCoroutine = null;
    }
}