using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeSystem : MonoBehaviour
{
    public static PlayerUpgradeSystem Instance;

    [Header("Upgrade Pool")]
    public List<UpgradeData> upgradePool;

    // Event gửi UI
    public event Action<List<UpgradeData>> OnShowUpgradeUI;

    private CharacterStats stats;
    private PlayerLevelSystem levelSystem;

    // =========================
    // STAT UPGRADE (NHIỀU CẤP)
    // =========================
    private Dictionary<StatType, StatUpgradeProgress> statUpgrades
        = new Dictionary<StatType, StatUpgradeProgress>();

    // =========================
    // SKILL UPGRADE
    // =========================
    private List<BaseSkill> unlockedSkills = new List<BaseSkill>();

    #region Unity

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (levelSystem != null)
            levelSystem.OnLevelUp -= HandleLevelUp;
    }

    #endregion

    #region Init

    public void Init(PlayerLevelSystem level, CharacterStats characterStats)
    {
        levelSystem = level;
        stats = characterStats;

        levelSystem.OnLevelUp += HandleLevelUp;
    }

    #endregion

    #region Level Up

    private void HandleLevelUp(int newLevel)
    {
        ShowRandomUpgrades();
    }

    private void ShowRandomUpgrades()
    {
        if (upgradePool.Count == 0)
            return;

        List<UpgradeData> poolCopy = new List<UpgradeData>(upgradePool);

        // Shuffle
        for (int i = poolCopy.Count - 1; i > 0; i--)
        {
            int rand = UnityEngine.Random.Range(0, i + 1);
            (poolCopy[i], poolCopy[rand]) = (poolCopy[rand], poolCopy[i]);
        }

        List<UpgradeData> result =
            poolCopy.GetRange(0, Mathf.Min(3, poolCopy.Count));

        OnShowUpgradeUI?.Invoke(result);
    }

    #endregion

    #region Apply Upgrade

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.Stat:
                ApplyStatUpgrade(upgrade.statUpgradeData);
                break;

            case UpgradeType.Skill:
                ApplySkillUpgrade(upgrade.skillUpgradeData);
                break;
        }

        stats.RecalculateStats();
    }

    #endregion

    // =========================================================
    // STAT UPGRADE
    // =========================================================

    private void ApplyStatUpgrade(StatUpgradeData data)
    {
        if (data == null) return;

        var type = data.statType;

        if (!statUpgrades.ContainsKey(type))
        {
            statUpgrades[type] = new StatUpgradeProgress
            {
                data = data,
                level = 0
            };
        }

        var progress = statUpgrades[type];

        if (progress.level >= data.MaxLevel)
            return;

        progress.level++;
        statUpgrades[type] = progress;
    }

    public float GetStatBonus(StatType type)
    {
        if (!statUpgrades.ContainsKey(type))
            return 0;

        var progress = statUpgrades[type];
        return progress.data.GetValue(progress.level);
    }

    // =========================================================
    // SKILL UPGRADE
    // =========================================================

    private void ApplySkillUpgrade(SkillUpgradeData data)
    {
        if (data == null || data.skillPrefab == null)
            return;

        // Đã có skill → level up
        foreach (var skill in unlockedSkills)
        {
            if (skill.UpgradeData == data)
            {
                skill.OnLevelUp();
                return;
            }
        }

        // Skill mới
        GameObject skillGO = Instantiate(data.skillPrefab, transform);
        var newSkill = skillGO.GetComponent<BaseSkill>();

        if (newSkill == null)
        {
            Debug.LogError("Skill prefab không có BaseSkill!");
            Destroy(skillGO);
            return;
        }

        newSkill.Init(stats);
        newSkill.OnUnlock();

        unlockedSkills.Add(newSkill);
    }
    public int GetStatLevel(StatType stat)
    {
        if (!statUpgrades.ContainsKey(stat))
            return 0;

        return statUpgrades[stat].level;
    }
    public int GetSkillLevel(SkillUpgradeData data)
    {
        foreach (var skill in unlockedSkills)
        {
            if (skill.UpgradeData == data)
                return skill.Level; // hoặc expose getter
        }
        return 0;
    }

    #region Backward Compatibility (CharacterStats)

    public int GetBonusHealth() => Mathf.RoundToInt(GetStatBonus(StatType.MaxHealth));
    public int GetBonusDamage() => Mathf.RoundToInt(GetStatBonus(StatType.Damage));
    public float GetBonusMoveSpeed() => GetStatBonus(StatType.MoveSpeed);
    public float GetBonusAttackSpeed() => GetStatBonus(StatType.AttackSpeed);
    public float GetBonusCrit() => 0;

    #endregion
}
