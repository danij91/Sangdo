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
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("LoadSceneAsync: sceneName is null or empty.");
            yield break;
        }
        
        // 1. 필요하면 로딩 UI 표시
        // UIManager.Instance.OpenScreen<LoadingUI>();  // 가상의 LoadingUI

        // 2. 씬 로딩 시작
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        // 페이드아웃 연출 등 추가할 경우, op.allowSceneActivation을 false로 관리할 수 있음.
        op.allowSceneActivation = true;

        // 3. 로딩 진행률 확인 (예: 진행 바 업데이트)
        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            // UIManager.Instance.loadingUI.SetProgress(progress);
            yield return null;
        }

        // 4. 새 씬 활성화 완료됨
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