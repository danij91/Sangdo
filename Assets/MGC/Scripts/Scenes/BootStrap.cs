using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject[] managerPrefabs;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        foreach (var prefab in managerPrefabs)
        {
            if (prefab != null)
            {
                var go = Instantiate(prefab);
                DontDestroyOnLoad(go);
            }
        }
    }

    private void Start()
    {
        LoadLobbyScene();
    }

    private void LoadLobbyScene()
    {
        SceneManager.Instance.LoadScene(SceneType.Lobby);
    }
}