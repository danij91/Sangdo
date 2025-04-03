using UnityEngine;

public class TownEntity : MonoBehaviour, IInteractable
{
    public string name;
    public void Interact()
    {
        Debug.Log($"마을 {name} 도착 → Town UI 열기");
        // UIManager.Instance.OpenScreen<TownMainScreen>();
    }
    
    public void OnSelect()
    {
        Debug.Log($"{name} 선택됨");
    }
}