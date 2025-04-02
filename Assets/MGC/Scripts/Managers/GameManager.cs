using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    protected override void OnInitialize()
    {
        // // 필요한 다른 매니저가 붙어있는지 확인하거나 동적으로 추가
        // if (FindFirstObjectByType<UIManager>() == null)
        //     gameObject.AddComponent<UIManager>();  // 예: 동적으로 UIManager 추가
        // if (FindFirstObjectByType<SceneManager>() == null)
        //     gameObject.AddComponent<SceneManager>();  // 예: 동적으로 SceneManager 추가
    }

    void Start()
    {

    }

    public void StartGame()
    {
        // SceneManager.Instance.LoadScene("World");
    }

    // 게임 종료 또는 메인 메뉴로 돌아가기
    public void ReturnToLobby()
    {
        // SceneManager.Instance.LoadScene("Lobby");
    }
}

