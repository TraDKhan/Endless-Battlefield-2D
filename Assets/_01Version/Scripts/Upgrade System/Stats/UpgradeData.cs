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
    public abstract int GetCurrentLevel();
    public abstract string GetTitle();
    public abstract string GetDescription();
    public virtual string GetLevelText() {
        return $"Lv {GetCurrentLevel()}";
    }
}
