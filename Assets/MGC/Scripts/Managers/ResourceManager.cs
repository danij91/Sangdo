using UnityEngine;

public class ResourceManager : SingletonMono<ResourceManager>
{
    /// <summary>
    /// Resources 폴더에서 특정 타입의 프리팹을 불러옵니다.
    /// 예: LoadUI<MainMenuUI>() → Resources/UI/MainMenuUI.prefab
    /// </summary>
    public GameObject LoadUI<T>() where T : Component
    {
        string path = $"UI/{typeof(T).Name}";
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab == null)
        {
            Debug.LogError($"[ResourceManager] UI 프리팹을 찾을 수 없습니다: {path}");
            return null;
        }

        return prefab;
    }

    public GameObject LoadUI<T>(string dir) where T : Component
    {
        string path = $"UI/{dir}/{typeof(T).Name}";
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab == null)
        {
            Debug.LogError($"[ResourceManager] UI 프리팹을 찾을 수 없습니다: {path}");
            return null;
        }

        return prefab;
    }

    /// <summary>
    /// 경로를 통해 GameObject 프리팹 로드 후 인스턴스 반환
    /// </summary>
    public GameObject Load(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"[ResourceManager] 로드 실패: {path}");
            return null;
        }
        return prefab;
    }

    /// <summary>
    /// 일반 오브젝트 (예: 텍스처, 스프라이트 등) 로드
    /// </summary>
    public T LoadAsset<T>(string path) where T : Object
    {
        T asset = Resources.Load<T>(path);
        if (asset == null)
        {
            Debug.LogError($"[ResourceManager] 에셋 로드 실패: {path}");
        }
        return asset;
    }

    public T LoadPrefab<T>() where T : Object
    {
        string path = $"Prefabs/{typeof(T).Name}";

        T asset = Resources.Load<T>(path);
        if (asset == null)
        {
            Debug.LogError($"[ResourceManager] 에셋 로드 실패: {path}");
        }
        return asset;
    }

    protected override void OnInitialize()
    {
    }
}
