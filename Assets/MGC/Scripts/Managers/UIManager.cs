using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : SingletonMono<UIManager>
{
    public Transform screenRoot;
    public Transform popupRoot;
    public Transform overlayRoot;

    private UIBase currentScreen;
    private UIBase currentOverlay;
    private Stack<UIBase> popupStack = new Stack<UIBase>();

    protected override void OnInitialize()
    {
        // UI Root 없으면 생성
        CreateUIRootIfNeeded();

        // EventSystem 없으면 생성
        CreateEventSystemIfNeeded();
    }

    private void CreateUIRootIfNeeded()
    {
        GameObject canvasPrefab = ResourceManager.Instance.Load("UI/Canvas");
        Canvas canvas = Instantiate(canvasPrefab, transform).GetComponent<Canvas>();

        // DontDestroyOnLoad(canvas.gameObject);
        var canvasTransform = canvas.transform;
        screenRoot = canvasTransform.Find("ScreenRoot");
        popupRoot = canvasTransform.Find("PopupRoot");
        overlayRoot = canvasTransform.Find("OverlayRoot");
    }

    private void CreateEventSystemIfNeeded()
    {
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
            DontDestroyOnLoad(es);
        }
    }

    public T OpenScreen<T>() where T : UIBase
    {
        if (currentScreen != null)
        {
            Destroy(currentScreen.gameObject);
            currentScreen = null;
        }

        var uiPrefab = ResourceManager.Instance.LoadUI<T>("Screen");
        var ui = Instantiate(uiPrefab, screenRoot).GetComponent<T>();
        currentScreen = ui;
        return ui;
    }

    public T OpenPopup<T>() where T : UIBase
    {
        var uiPrefab = ResourceManager.Instance.LoadUI<T>("Popup");
        var ui = Instantiate(uiPrefab, screenRoot).GetComponent<T>();
        currentScreen = ui;
        popupStack.Push(ui);
        return ui;
    }
    
    public T OpenOverlay<T>() where T : UIBase
    {
        if (currentOverlay is T overlay)
        {
            return overlay;
        }
        
        if (currentOverlay != null)
        {
            Destroy(currentOverlay.gameObject);
            currentOverlay = null;
        }

        var prefab = ResourceManager.Instance.LoadUI<T>("Overlay");
        var ui = Instantiate(prefab, overlayRoot).GetComponent<T>();
        currentOverlay = ui;
        return ui;
    }

    public void CloseScreen()
    {
        if (currentScreen != null)
        {
            Destroy(currentScreen.gameObject);
            currentScreen = null;
        }
    }

    public void CloseOverlay()
    {
        if (currentOverlay != null)
        {
            Destroy(currentOverlay.gameObject);
            currentOverlay = null;
        }
    }

    public void ClosePopup()
    {
        if (popupStack.Count == 0) return;
        var popup = popupStack.Pop();
        Destroy(popup.gameObject);
    }

    public void CloseAllPopups()
    {
        while (popupStack.Count > 0)
        {
            ClosePopup();
        }
    }
}
