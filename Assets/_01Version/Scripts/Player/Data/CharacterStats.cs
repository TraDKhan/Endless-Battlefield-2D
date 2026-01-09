using System;
using UnityEngine;

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
    private UpgradeSystem upgradeSystem;

    public event Action OnStatsChanged;

    public CharacterStats(
        PlayerData data,
        PlayerLevelSystem level,
        PlayerEquipmentController equipment,
        PlayerBuffController buff,
        UpgradeSystem upgrade)
    {
        playerData = data;
        levelSystem = level;
        equipmentController = equipment;
        buffController = buff;
        upgradeSystem = upgrade;
    }

    public void RecalculateStats()
    {
        // ===== 1. BASE =====
        maxHealth = playerData.baseHealth;
        maxEnergy = playerData.baseEnergy;
        armor = playerData.baseArmor;
        moveSpeed = playerData.baseMoveSpeed;

        // ===== 2. LEVEL =====
        if (levelSystem != null)
        {
            maxHealth += levelSystem.GetBonusHealth();
        }

        // ===== 3. EQUIPMENT =====
        if (equipmentController != null)
        {
            maxHealth += equipmentController.GetEquipHealthBonus();
            armor += equipmentController.GetEquipArmorBonus();
            moveSpeed += equipmentController.GetEquipMoveSpeedBonus();
        }

        // ===== 4. BUFF =====
        if (buffController != null)
        {
            maxHealth += buffController.GetBuffHealthBonus();
            moveSpeed += buffController.GetBuffMoveSpeedBonus();
        }

        // ===== 5. PLAYER UPGRADE =====
        if (upgradeSystem != null)
        {
            maxHealth += Mathf.RoundToInt(
                upgradeSystem.CalculatePlayerStatBonus(PlayerStatType.MaxHealth)
            );

            maxEnergy += Mathf.RoundToInt(
                upgradeSystem.CalculatePlayerStatBonus(PlayerStatType.Energy)
            );

            moveSpeed += upgradeSystem.CalculatePlayerStatBonus(PlayerStatType.MoveSpeed);

            armor += Mathf.RoundToInt(
                upgradeSystem.CalculatePlayerStatBonus(PlayerStatType.Armor)
            );
        }

        OnStatsChanged?.Invoke();
    }

    // ===== GETTER =====
    public int GetMaxHealth() => maxHealth;
    public float GetMoveSpeed() => moveSpeed;
    public int GetArmor() => armor;
    public int GetMaxEnergy() => maxEnergy;

}
