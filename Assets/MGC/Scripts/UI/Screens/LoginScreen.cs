using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : ScreenBase
{
    [SerializeField] private Button loginButton;

    private void Awake()
    {
        loginButton.onClick.AddListener(OnLoginButtonPressed);
    }

    private void OnDestroy()
    {
        loginButton.onClick.RemoveAllListeners(); // 메모리 해제 안전
    }
    
    private void OnLoginButtonPressed()
    {
        LobbySceneController controller = FindObjectOfType<LobbySceneController>();
        controller.OnLoginSuccess();
    }
}