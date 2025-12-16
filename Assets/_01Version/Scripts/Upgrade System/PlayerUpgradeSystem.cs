using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStatUpgrade
{
    public float valuePerLevel;
    public int level;

    public float GetValue() => valuePerLevel * level;
}

public class PlayerUpgradeSystem : MonoBehaviour
{
    public static PlayerUpgradeSystem Instance;

    [Header("Upgrade Pool")]
    public List<UpgradeData> upgradePool;

    public event Action<List<UpgradeData>> OnShowUpgradeUI;
    private Dictionary<PlayerStatType, PlayerStatUpgrade> playerStats
        = new();

    private void Awake()
    {
        Instance = this;
    }

    // ================= LEVEL UP =================
    public void OnLevelUp()
    {
        List<UpgradeData> options = GetRandomUpgrades(3);
        OnShowUpgradeUI?.Invoke(options);
    }

    // ================= RANDOM KHÔNG TRÙNG =================
    List<UpgradeData> GetRandomUpgrades(int count)
    {
        List<UpgradeData> validPool = new List<UpgradeData>();

        foreach (var up in upgradePool)
        {
            if (up.CanApply())
                validPool.Add(up);
        }

        List<UpgradeData> result = new List<UpgradeData>();

        while (result.Count < count && validPool.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, validPool.Count);
            result.Add(validPool[index]);
            validPool.RemoveAt(index);
        }

        return result;
    }

    // ================= APPLY =================
    public void ApplyUpgrade(UpgradeData data)
    {
        data.Apply();
    }

    // ===== PLAYER STAT =====
    Dictionary<PlayerStatType, int> statLevels = new();

    public void ApplyPlayerStat(PlayerStatType stat, float value)
    {
        if (!playerStats.ContainsKey(stat))
        {
            playerStats[stat] = new PlayerStatUpgrade
            {
                valuePerLevel = value,
                level = 0
            };
        }

        playerStats[stat].level++;

        CharacterStatsController.Instance.Stats.RecalculateStats();
    }
    public float GetPlayerStatBonus(PlayerStatType stat)
    {
        return playerStats.TryGetValue(stat, out var s)
            ? s.GetValue()
            : 0f;
    }
    public int GetPlayerStatLevel(PlayerStatType stat)
        => statLevels.ContainsKey(stat) ? statLevels[stat] : 0;

    // ===== SKILL =====
    List<BaseSkill> skills = new();

    public void ApplySkillUpgrade(SkillUpgradeData data)
    {
        foreach (var s in skills)
        {
            if (s.UpgradeData == data)
            {
                s.OnLevelUp();
                return;
            }
        }

        var go = Instantiate(data.skillPrefab);
        var skill = go.GetComponent<BaseSkill>();

        skill.Init(
            CharacterStatsController.Instance.transform,
            CharacterStatsController.Instance.Stats
        );

        skill.OnUnlock();
        skills.Add(skill);
    }

    public int GetSkillLevel(SkillUpgradeData data)
    {
        foreach (var s in skills)
            if (s.UpgradeData == data)
                return s.Level;

        return 0;
    }
}
