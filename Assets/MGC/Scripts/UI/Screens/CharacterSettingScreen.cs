using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectScreen : ScreenBase
{
    [SerializeField] private Button selectButton;

    private void Awake()
    {
        if (selectButton != null)
        {
            selectButton.onClick.AddListener(() =>
            {
                Debug.Log("[CharacterSelectScreen] Character selected.");
                LoginManager.Instance.SelectCharacter();
            });
        }
    }
}