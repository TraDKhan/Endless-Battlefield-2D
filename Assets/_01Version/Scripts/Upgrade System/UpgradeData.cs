using UnityEngine;

public abstract class UpgradeData : ScriptableObject
{
    [Header("Info")]
    public string upgradeName;
    public Sprite icon;
    public UpgradeType upgradeType;

    [TextArea]
    public string description;

    // ===== LOGIC =====
    public abstract bool CanApply(UpgradeSystem system);
    public abstract void Apply(UpgradeSystem system);

    // ===== UI =====
    public abstract int GetCurrentLevel(UpgradeSystem system);

    public virtual string GetTitle() => upgradeName;

    public virtual string GetTypeText() => upgradeType.ToString();

    public virtual string GetLevelText(UpgradeSystem system)
        => $"Lv {GetCurrentLevel(system)}";

    public abstract string GetValueText(UpgradeSystem system);
    public virtual string GetDescription() => description;
}
