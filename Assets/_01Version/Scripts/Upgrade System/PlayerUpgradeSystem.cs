using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeSystem : MonoBehaviour
{
    public static PlayerUpgradeSystem Instance;

    [Header("Upgrade Pool (ScriptableObject)")]
    public List<UpgradeData> upgradePool;

    // Event gửi UI 3 lựa chọn random
    public event Action<List<UpgradeData>> OnShowUpgradeUI;

    private CharacterStats stats;

    // Lưu bonus stat
    private Dictionary<StatType, float> statBonuses = new Dictionary<StatType, float>();

    // Lưu skill đã unlock
    private List<GameObject> unlockedSkills = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void Init(PlayerLevelSystem level, CharacterStats characterStats)
    {
        stats = characterStats;

        // Lắng nghe level up
        level.OnLevelUp += HandleLevelUp;
    }

    private void OnDestroy()
    {
        // Không leak event
        var level = FindAnyObjectByType<PlayerLevelSystem>();
        if (level != null)
            level.OnLevelUp -= HandleLevelUp;
    }

    private void HandleLevelUp(int newLevel)
    {
        ShowRandomUpgrades();
    }

    // Random 3 upgrade
    void ShowRandomUpgrades()
    {
        // Copy tránh làm thay đổi danh sách gốc
        List<UpgradeData> poolCopy = new List<UpgradeData>(upgradePool);

        // Fisher-Yates Shuffle
        for (int i = poolCopy.Count - 1; i > 0; i--)
        {
            int rand = UnityEngine.Random.Range(0, i + 1);
            (poolCopy[i], poolCopy[rand]) = (poolCopy[rand], poolCopy[i]);
        }

        // Lấy 3 lựa chọn đầu tiên
        List<UpgradeData> result = poolCopy.GetRange(0, Mathf.Min(3, poolCopy.Count));

        OnShowUpgradeUI?.Invoke(result);
    }


    // --------------------------
    // ÁP DỤNG UPGRADE
    // --------------------------

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade.upgradeType == UpgradeType.Stat)
        {
            ApplyStatUpgrade(upgrade);
        }
        else if (upgrade.upgradeType == UpgradeType.Skill)
        {
            ApplySkillUpgrade(upgrade);
        }

        // Cập nhật character stats
        stats.RecalculateStats();
    }

    // --- STAT UPGRADE ---
    private void ApplyStatUpgrade(UpgradeData upgrade)
    {
        if (!statBonuses.ContainsKey(upgrade.statType))
            statBonuses[upgrade.statType] = 0;

        statBonuses[upgrade.statType] += upgrade.statValue;
    }

    // --- SKILL UPGRADE ---
    private void ApplySkillUpgrade(UpgradeData upgrade)
    {
        if (upgrade.skillPrefab == null)
        {
            Debug.LogWarning("Skill upgrade nhưng không có skillPrefab!");
            return;
        }

        // Gắn skill vào Player
        GameObject newSkill = Instantiate(upgrade.skillPrefab, transform);
        unlockedSkills.Add(newSkill);

        Debug.Log("Unlocked Skill: " + upgrade.upgradeName);
    }

    // --------------------------
    // GET BONUS cho CharacterStats
    // --------------------------

    public float GetStatBonus(StatType type)
    {
        if (statBonuses.ContainsKey(type))
            return statBonuses[type];

        return 0;
    }

    // Các hàm cũ tương thích CharacterStats
    public int GetBonusHealth() => (int)GetStatBonus(StatType.MaxHealth);
    public int GetBonusDamage() => (int)GetStatBonus(StatType.Damage);
    public float GetBonusMoveSpeed() => GetStatBonus(StatType.MoveSpeed);
    public float GetBonusAttackSpeed() => GetStatBonus(StatType.AttackSpeed);
    public float GetBonusCrit() => 0; // chưa dùng trong cấu trúc mới
}
