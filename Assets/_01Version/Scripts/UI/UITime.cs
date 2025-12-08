using TMPro;
using UnityEngine;

public class UITime : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private void Start()
    {
        UpdateUI();

        if (TimeController.Instance != null)
            TimeController.Instance.OnTimeChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        if (TimeController.Instance != null)
            TimeController.Instance.OnTimeChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (TimeController.Instance == null) return;

        timeText.text = TimeController.Instance.GetTimeString();
    }
}
