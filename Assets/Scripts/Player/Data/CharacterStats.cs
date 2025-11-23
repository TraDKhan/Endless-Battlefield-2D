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

    public CharacterStats(
        Player data,
        PlayerLevelSystem level,
        PlayerEquipmentController equipment,
        PlayerBuffController buff)
    {
        playerData = data;
        levelSystem = level;
        equipmentController = equipment;
        buffController = buff;
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
    }
}
