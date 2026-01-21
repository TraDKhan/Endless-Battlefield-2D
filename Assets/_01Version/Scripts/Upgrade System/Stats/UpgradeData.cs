using UnityEngine;

public abstract class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;
    public UpgradeType upgradeType;

    [TextArea]
    public string description;

    public abstract bool CanApply();

    public abstract void Apply();

    // ===== GỬI CHO UI ===== \\

    public abstract int GetCurrentLevel();

    public abstract string GetTitle();

    public virtual string GetTypeText()
    {
        return $"{upgradeType.ToString()}";
    }

    public virtual string GetLevelText() {
        return $"Lv {GetCurrentLevel()}";
    }

    public abstract string GetValueText();

    public abstract string GetDescription();
}
