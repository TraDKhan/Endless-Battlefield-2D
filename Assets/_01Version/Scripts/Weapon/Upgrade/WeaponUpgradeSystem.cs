using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour
{
    private PlayerLevelSystem levelSystem;
    private Weapon weapon;

    private int bonusDamage;
    private float bonusCrit;
    private float bonusCooldown;

    public void Init(PlayerLevelSystem level, Weapon weaponRef)
    {
        levelSystem = level;
        weapon = weaponRef;

        levelSystem.OnLevelUp += OnLevelUp;
    }

    private void OnDestroy()
    {
        if (levelSystem != null)
            levelSystem.OnLevelUp -= OnLevelUp;
    }

    private void OnLevelUp(int level)
    {
        // Ví dụ
        bonusDamage += 5;
        bonusCrit += 0.05f;

        //weapon.RecalculateStats();
    }

    // ===== API =====
    public int GetBonusDamage() => bonusDamage;
    public float GetBonusCrit() => bonusCrit;
    public float GetBonusCooldown() => bonusCooldown;
}
