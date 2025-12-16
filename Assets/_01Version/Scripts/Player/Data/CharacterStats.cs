using System;

public class CharacterStats
{
    public int maxHealth;
    public int maxEnergy;
    public int armor;
    public float moveSpeed;

    private PlayerData playerData;

    private PlayerLevelSystem levelSystem;
    private PlayerEquipmentController equipmentController;
    private PlayerBuffController buffController;
    private PlayerUpgradeSystem upgradeController;

    public event Action OnStatsChanged;

    public CharacterStats(
        PlayerData data,
        PlayerLevelSystem level,
        PlayerEquipmentController equipment,
        PlayerBuffController buff,
        PlayerUpgradeSystem upgrade)
    {
        playerData = data;
        levelSystem = level;
        equipmentController = equipment;
        buffController = buff;
        upgradeController = upgrade;
    }

    public void RecalculateStats()
    {
        // 1. Base
        maxHealth = playerData.baseHealth;
        maxEnergy = playerData.baseEnergy;
        armor = playerData.baseArmor;
        moveSpeed = playerData.baseMoveSpeed;

        // 2. Level
        if (levelSystem != null)
        {
            maxHealth += levelSystem.GetBonusHealth();
        }

        // 3. Equipment
        if (equipmentController != null)
        {
            maxHealth += equipmentController.GetEquipHealthBonus();
            armor += equipmentController.GetEquipArmorBonus();
            moveSpeed += equipmentController.GetEquipMoveSpeedBonus();
        }

        // 4. Buff
        if (buffController != null)
        {
            maxHealth += buffController.GetBuffHealthBonus();
            moveSpeed += buffController.GetBuffMoveSpeedBonus();
        }

        // 5. Upgrade (PLAYER upgrade, không phải weapon)
        if (upgradeController != null)
        {
            maxHealth += upgradeController.GetBonusHealth();
            moveSpeed += upgradeController.GetBonusMoveSpeed();
        }

        OnStatsChanged?.Invoke();
    }

    public int GetMaxHealth() => maxHealth;
    public float GetMoveSpeed() => moveSpeed;
    public int GetArmor() => armor;
}
