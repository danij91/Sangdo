using UnityEngine;

public class LoginManager : SingletonMono<LoginManager>
{
    public bool IsLoggedIn { get; private set; } = false;
    public bool HasSelectedCharacter { get; private set; } = false;

    public void DummyLogin()
    {
        IsLoggedIn = true;
        Debug.Log("[LoginManager] Dummy login successful.");
    }

    public void SelectCharacter()
    {
        HasSelectedCharacter = true;
        Debug.Log("[LoginManager] Dummy character selected.");
    }

    protected override void OnInitialize() { }
}
