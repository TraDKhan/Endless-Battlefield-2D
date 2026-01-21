using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpgradeOption : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button button;

    private UpgradeData data;

    public void SetData(UpgradeData upgradeData, System.Action onClick)
    {
        data = upgradeData;

        if (icon != null)
            icon.sprite = data.icon;

        if (title != null)
            title.text = data.GetTitle();

        if(typeText != null)
            typeText.text = data.GetTypeText();

        if (levelText != null)
            levelText.text = data.GetLevelText();

        if (valueText != null)
            valueText.text = data.GetValueText();

        if (description != null)
            description.text = data.GetDescription();

        button.interactable = data.CanApply();

        BindButton(onClick);
    }

    void BindButton(System.Action onClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}
