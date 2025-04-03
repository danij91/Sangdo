using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PlayerMover))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap unwalkableTilemap;

    private PlayerMover mover;
    private PathFinder pathFinder;

    private void Awake()
    {
        mover = GetComponent<PlayerMover>();
        pathFinder = GetComponent<PathFinder>();
    }

    public void Initialize(Tilemap walkableTilemap, Tilemap unwalkableTilemap)
    {
        this.walkableTilemap = walkableTilemap;
        this.unwalkableTilemap = unwalkableTilemap;
    }

    private void Start()
    {
        pathFinder.SetTilemaps(walkableTilemap, unwalkableTilemap);
        mover.SetWalkableTilemap(walkableTilemap);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = walkableTilemap.WorldToCell(worldPosition);
            Vector2Int targetTile = (Vector2Int)cellPosition;
            Vector2Int currentTile = (Vector2Int)walkableTilemap.WorldToCell(transform.position);

            Collider2D hit = Physics2D.OverlapPoint(worldPosition);
            IInteractable target = null;

            if (hit != null)
            {
                target = hit.GetComponent<IInteractable>();
                if (target != null)
                {
                    target.OnSelect();
                    targetTile = (Vector2Int)walkableTilemap.WorldToCell(hit.transform.position);
                }
            }

            mover.MoveTo(currentTile, targetTile, target);
        }
    }
}