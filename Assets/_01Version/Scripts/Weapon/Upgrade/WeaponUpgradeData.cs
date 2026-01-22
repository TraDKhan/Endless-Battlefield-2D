using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Stat Weapon")]
public class WeaponUpgradeData : UpgradeData
{
    [Header("Stat")]
    public StatType statType;
    public StatModType modType;
    public float valuePerLevel;

    // ======================
    // LOGIC
    // ======================

    public override bool CanApply(UpgradeSystem system)
    {
        return true; // sau này có thể limit
    }

    public override void Apply(UpgradeSystem system)
    {
        system.Weapon.ApplyUpgrade(statType, valuePerLevel, modType);
    }

    // ======================
    // UI
    // ======================

    public override int GetCurrentLevel(UpgradeSystem system)
    {
        return system.Weapon.GetStatLevel(statType);
    }

    public override string GetValueText(UpgradeSystem system)
    {
        float value = system.Weapon.GetStatValue(statType);
        string sign = modType == StatModType.Percent ? "%" : "";

        return $"+{value}{sign}";
    }

    public override string GetDescription()
    {
        return description;
    }
}
