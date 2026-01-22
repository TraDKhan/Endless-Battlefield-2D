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
        system.ApplyWeaponStatsUpgrade(this);
    }

    // ======================
    // UI
    // ======================

    public override int GetCurrentLevel(UpgradeSystem system)
    {
        return system.Weapon.GetStatLevel(statType);
    }
    public override string GetLevelText(UpgradeSystem system)
    {
        int current = GetCurrentLevel(system) + 1;
        int next = current + 1;

        return $"Lv{current} → Lv{next}";
    }

    public override string GetValueText(UpgradeSystem system)
    {
        string sign = modType == StatModType.Percent ? "%" : "";

        // UI upgrade CHỈ hiển thị giá trị tăng
        return $"+{valuePerLevel}{sign} {statType.ToString()}";
    }

    public override string GetDescription()
    {
        return description;
    }
}
