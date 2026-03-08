using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Stat Weapon")]
public class WeaponUpgradeData : UpgradeData
{
    [Header("Stat")]
    public WeaponStatType statType;
    public float valuePerLevel;

    // ======================
    // LOGIC
    // ======================

    public override bool CanApply(UpgradeSystem system)
    {
        return true;
    }

    public override void Apply(UpgradeSystem system)
    {
        Debug.Log("Weapon Stat");
        system.WeaponSystem.Apply(this);
    }

    // ======================
    // UI
    // ======================

    public override int GetCurrentLevel(UpgradeSystem system)
    {
        return system.WeaponSystem.GetLevel(statType);
    }

    public override string GetLevelText(UpgradeSystem system)
    {
        int current = GetCurrentLevel(system) + 1;
        int next = current + 1;

        return $"Lv{current} → Lv{next}";
    }

    public override string GetValueText(UpgradeSystem system)
    {
        return $"+{valuePerLevel} {statType.ToString()}";
    }

    public override string GetDescription()
    {
        return description;
    }
}
