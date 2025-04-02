using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : ScreenBase
{
    
    [SerializeField] private Button loginButton;

    private void Awake()
    {
        loginButton = GetComponentInChildren<Button>();
        if (loginButton != null)
        {
            loginButton.onClick.AddListener(() =>
            {
                LoginManager.Instance.DummyLogin();
            });
        }
    }
}