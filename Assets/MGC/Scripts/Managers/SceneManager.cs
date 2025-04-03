using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Enums;

public class SceneManager : SingletonMono<SceneManager>
{
    /// <summary>지정한 씬으로 전환 (비동기 로딩 예시)</summary>
    public void LoadScene(SceneType sceneType)
    {
        StartCoroutine(LoadSceneAsync(sceneType.ToString()));
    }

    public string GetCurrentSceneName()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    // 비동기 씬 로딩 코루틴
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // 1. 로딩 화면 열기
        var loadingScreen = UIManager.Instance.OpenScreen<LoadingScreen>();

        // 2. 씬 로딩 시작
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            loadingScreen.SetProgress(op.progress / 0.9f);
            yield return null;
        }

        // 3. 로딩 완료 처리
        loadingScreen.SetProgress(1f);
        yield return new WaitForSeconds(0.3f); // 짧은 대기

        op.allowSceneActivation = true;
        yield return new WaitUntil(() => op.isDone);

        // 4. 씬 변경 후 처리
        OnSceneChanged(sceneName);
    }

    
    // 새 씬 진입 후 처리 (필요 시 UI 세팅 등을 수행)
    private void OnSceneChanged(string sceneName)
    {
        // 로딩 UI 닫기
        UIManager.Instance.CloseAllPopups();
        // 씬 이름에 따라 다른 UI 표시 등 분기 가능
        // if (sceneName == "Lobby")
        // {
        //     UIManager.Instance.OpenScreen<MainMenuUI>();
        // }
        // else if (sceneName == "World" || sceneName == "Battle" || sceneName == "Town")
        // {
        //     UIManager.Instance.OpenScreen<InGameUI>();
        // }
        var controller = Object.FindFirstObjectByType<BaseSceneController>();
        controller?.Initialize();
    }

    protected override void OnInitialize()
    {
        // 추가 초기화가 필요하다면 여기에 작성
    }
}