using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpgradeOption : MonoBehaviour
{
    [Header("UI")]
    public Image icon;
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text levelText;     // (Lv x / y)
    public Button button;

    private UpgradeData currentData;

    public void SetData(UpgradeData data, PlayerUpgradeSystem upgradeSystem)
    {
        currentData = data;

        switch (data.upgradeType)
        {
            case UpgradeType.Stat:
                SetupStatUpgrade(data.statUpgradeData, upgradeSystem);
                break;

            case UpgradeType.Skill:
                SetupSkillUpgrade(data.skillUpgradeData, upgradeSystem);
                break;
        }
    }

    // =========================
    // STAT UI
    // =========================
    private void SetupStatUpgrade(
        StatUpgradeData data,
        PlayerUpgradeSystem upgradeSystem
    )
    {
        if (data == null) return;

        if (icon != null)
            icon.sprite = data.icon;

        if (title != null)
            title.text = data.statType.ToString();

        int currentLevel = upgradeSystem.GetStatLevel(data.statType);
        int nextLevel = Mathf.Min(currentLevel + 1, data.MaxLevel);

        float nextValue = data.GetValue(nextLevel);

        if (description != null)
            description.text = $"+{nextValue} {data.statType.ToString()}";

        if (levelText != null)
            levelText.text = $"Lv {currentLevel}/{data.MaxLevel}";
    }

    // =========================
    // SKILL UI
    // =========================
    private void SetupSkillUpgrade(
        SkillUpgradeData data,
        PlayerUpgradeSystem upgradeSystem
    )
    {
        if (data == null) return;

        if (icon != null)
            icon.sprite = data.icon;

        if (title != null)
            title.text = data.skillName;

        int currentLevel = upgradeSystem.GetSkillLevel(data);
        int nextLevel = Mathf.Min(currentLevel + 1, data.MaxLevel);

        if (description != null)
        {
            if (currentLevel == 0)
                description.text = data.description;
            else
                description.text = $"Nâng cấp lên Lv {nextLevel}";
        }

        if (levelText != null)
            levelText.text = $"Lv {currentLevel}/{data.MaxLevel}";
    }

    // =========================
    // BUTTON
    // =========================
    public void BindButton(System.Action onClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());
    }
}
