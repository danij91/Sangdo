using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : ScreenBase
{
    [SerializeField] private TMP_Text loadingText;

    private float dotTimer = 0f;
    private int dotCount = 0;

    private void Update()
    {
        if (loadingText == null) return;

        dotTimer += Time.deltaTime;
        if (dotTimer >= 0.5f)
        {
            dotTimer = 0f;
            dotCount = (dotCount + 1) % 4;
            loadingText.text = "Loading" + new string('.', dotCount);
        }
    }
}