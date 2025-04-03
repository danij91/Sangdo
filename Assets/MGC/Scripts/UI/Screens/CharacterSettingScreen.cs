using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectScreen : ScreenBase
{
    [SerializeField] private Button slot1Button;
    [SerializeField] private Button slot2Button;
    [SerializeField] private Button slot3Button;

    private void Awake()
    {
        slot1Button.onClick.AddListener(() => OnCharacterSlotClicked(0));
        slot2Button.onClick.AddListener(() => OnCharacterSlotClicked(1));
        slot3Button.onClick.AddListener(() => OnCharacterSlotClicked(2));
    }

    private void OnDestroy()
    {
        slot1Button.onClick.RemoveAllListeners();
        slot2Button.onClick.RemoveAllListeners();
        slot3Button.onClick.RemoveAllListeners();
    }

    private void OnCharacterSlotClicked(int characterIndex)
    {
        Debug.Log($"[CharacterSelectScreen] 캐릭터 {characterIndex} 선택됨");

        LobbySceneController controller = FindObjectOfType<LobbySceneController>();
        controller.OnCharacterSelected();
    }
}