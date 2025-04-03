using UnityEngine;

public class LobbyOverlay : OverlayBase
{
    public void OnSettingsButtonPressed()
    {
        LobbySceneController controller = FindObjectOfType<LobbySceneController>();
        controller.OpenSettings();
    }
}