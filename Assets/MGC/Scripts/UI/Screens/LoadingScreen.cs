using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : ScreenBase
{
    // [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;

    public void SetProgress(float progress)
    {
        // progressBar.value = progress;
        progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
    }
}