using Enums;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMainScreen : ScreenBase
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;

    private void Awake()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(() =>
            {
                Debug.Log("[LobbyMainScreen] Start button clicked.");
                // 예: 월드 씬으로 전환
                SceneManager.Instance.LoadScene(SceneType.World);
            });
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(() =>
            {
                Debug.Log("[LobbyMainScreen] Settings button clicked.");
                UIManager.Instance.OpenPopup<LobbySettingsPopup>();
            });
        }
    }
}