using UnityEngine;
public abstract class UIBase : MonoBehaviour
{
    // UI 식별자나 종류를 표시할 수 있는 속성 (필요하다면)
    public string UIName => this.GetType().Name;

    // UI 열기: 기본적으로 GameObject 활성화
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    // UI 닫기: 기본적으로 GameObject 비활성화
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}