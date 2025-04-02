using UnityEngine;
using UnityEngine.Tilemaps;

public class HideTilemap : MonoBehaviour
{
    [SerializeField]
    private bool isHideTilemap;
    void Start()
    {
        if (isHideTilemap)
        {
            GetComponent<TilemapRenderer>().enabled = false;
        }
    }
}
