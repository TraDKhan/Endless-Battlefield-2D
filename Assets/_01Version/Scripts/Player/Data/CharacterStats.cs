using System;
using UnityEngine;
public class CharacterStats
{
    public int totalHealth;
    public int totalEnergy;
    public int totalArmor;
    public int totalDamage;
    public float totalMoveSpeed;
    public float totalAttackSpeed;
    public float totalCrit;


    private Player playerData;

    private PlayerLevelSystem levelSystem;
    private PlayerEquipmentController equipmentController;
    private PlayerBuffController buffController;
    private PlayerUpgradeSystem upgradeController;

    //event thay doi
    public event Action OnStatsChanged;

    public CharacterStats(
        Player data,
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
        totalHealth = playerData.baseHealth;
        totalEnergy = playerData.baseEnergy;
        totalArmor = playerData.baseArmor;
        totalDamage = playerData.baseDamage;
        totalMoveSpeed = playerData.baseMoveSpeed;
        totalAttackSpeed = playerData.baseAttackSpeed;
        totalCrit = playerData.baseCrit;

        // 2. Level bonus
        if (levelSystem != null)
        {
            totalHealth += levelSystem.GetHealthBonus();
            totalDamage += levelSystem.GetDamageBonus();
        }

        // 3. Equipment bonus
        if (equipmentController != null)
        {
            totalHealth += equipmentController.GetEquipHealthBonus();
            totalDamage += equipmentController.GetEquipDamageBonus();
            totalMoveSpeed += equipmentController.GetEquipMoveSpeedBonus();
        }

        // 4. Buff bonus
        if (buffController != null)
        {
            totalHealth += buffController.GetBuffHealthBonus();
            totalDamage += buffController.GetBuffDamageBonus();
            totalMoveSpeed += buffController.GetBuffMoveSpeedBonus();
        }

        // 5. Upgrade bonus
        if (upgradeController != null)
        {
            totalHealth += upgradeController.GetBonusHealth();
            totalDamage += upgradeController.GetBonusDamage();
            totalMoveSpeed += upgradeController.GetBonusMoveSpeed();
            totalAttackSpeed += upgradeController.GetBonusAttackSpeed();
            totalCrit += upgradeController.GetBonusCrit();
        }

        OnStatsChanged?.Invoke();
    }
    public int GetMaxHealth() => totalHealth;
    public int GetDamage() => totalDamage;
}
