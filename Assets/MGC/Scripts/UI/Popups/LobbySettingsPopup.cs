using UnityEngine;

public class LobbySettingsPopup : PopupBase
{
    public void OnCloseButtonPressed()
    {
        LobbySceneController controller = FindObjectOfType<LobbySceneController>();
        controller.ReturnToMain();
    }
}