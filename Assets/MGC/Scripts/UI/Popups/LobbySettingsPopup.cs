using UnityEngine;
using UnityEngine.UI;

public class LobbySettingsPopup : UIBase
{
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() =>
            {
                Debug.Log("[LobbySettingsPopup] Close button clicked.");
                UIManager.Instance.ClosePopup();
            });
        }
    }
}