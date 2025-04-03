// ✅ LobbyMainScreen.cs

using Enums;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMainScreen : ScreenBase
{
    [SerializeField] private Button enterWorldButton;
    [SerializeField] private Button settingsButton;

    private void Awake()
    {
        enterWorldButton.onClick.AddListener(OnEnterWorldPressed);
        settingsButton.onClick.AddListener(OnSettingsPressed);
    }

    private void OnDestroy()
    {
        enterWorldButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
    }

    private void OnEnterWorldPressed()
    {
        SceneManager.Instance.LoadScene(SceneType.World);
    }

    private void OnSettingsPressed()
    {
        LobbySceneController controller = FindFirstObjectByType<LobbySceneController>();
        controller.OpenSettings();
    }
}