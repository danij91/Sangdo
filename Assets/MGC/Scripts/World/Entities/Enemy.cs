using Enums;
using UnityEngine;

public class EnemyEntity : MonoBehaviour, IInteractable
{
    public string name;
    public void Interact()
    {
        Debug.Log($"적({name})과 마주침 → 전투 진입");
        // SceneManager.Instance.LoadScene(SceneType.Battle);
    }

    public void OnSelect()
    {
        Debug.Log($"{name} 선택됨");
    }
}