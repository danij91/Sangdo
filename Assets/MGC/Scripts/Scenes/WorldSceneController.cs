using System.Collections;
using UnityEngine;

public class WorldSceneController : BaseSceneController
{
    public override void Initialize()
    {
        Debug.Log("[World Scene] Initialized");

        var mapObject = ResourceManager.Instance.LoadPrefab<GameObject>("Map");
        Instantiate(mapObject);
    }

    // private IEnumerator Flow()
    // {
    // }
}