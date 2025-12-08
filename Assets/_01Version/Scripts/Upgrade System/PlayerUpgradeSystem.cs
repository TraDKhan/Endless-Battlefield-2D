using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeSystem : MonoBehaviour
{
    public static PlayerUpgradeSystem Instance;

    // Sự kiện gửi UI
    public event Action<List<UpgradeOption>> OnShowUpgradeUI;

    // Danh sách nâng cấp có thể random
    [Header("Upgrade Pool")]
    public List<UpgradeOption> upgradePool;

    private PlayerLevelSystem levelSystem;
    private CharacterStats stats;

    // Lưu bonus từ upgrade
    private int bonusHealth = 0;
    private int bonusDamage = 0;
    private float bonusMoveSpeed = 0;
    private float bonusAttackSpeed = 0;
    private float bonusCrit = 0;

    void Awake()
    {
        Instance = this;
    }

    public void Init(PlayerLevelSystem level, CharacterStats stat)
    {
        levelSystem = level;
        stats = stat;

        // Lắng nghe sự kiện lên cấp
        levelSystem.OnLevelUp += HandleLevelUp;
    }

    private void OnDestroy()
    {
        if (levelSystem != null)
            levelSystem.OnLevelUp -= HandleLevelUp;
    }

    private void HandleLevelUp(int newLevel)
    {
        ShowRandomUpgrades();
    }

    // Random 3 lựa chọn để hiện UI
    void ShowRandomUpgrades()
    {
        List<UpgradeOption> list = new List<UpgradeOption>();

        for (int i = 0; i < 3; i++)
        {
            var opt = upgradePool[UnityEngine.Random.Range(0, upgradePool.Count)];
            list.Add(opt);
        }

        OnShowUpgradeUI?.Invoke(list);
    }

    // Gọi khi người chơi chọn một upgrade
    public void ApplyUpgrade(UpgradeOption option)
    {
        switch (option.type)
        {
            case UpgradeType.Health:
                bonusHealth += (int)option.value;
                break;
            case UpgradeType.Damage:
                bonusDamage += (int)option.value;
                break;
            case UpgradeType.MoveSpeed:
                bonusMoveSpeed += option.value;
                break;
            case UpgradeType.AttackSpeed:
                bonusAttackSpeed += option.value;
                break;
            case UpgradeType.Crit:
                bonusCrit += option.value;
                break;
        }

        // Cập nhật lại chỉ số
        stats.RecalculateStats();
    }

    // Các hàm Stat được CharacterStats gọi
    public int GetBonusHealth() => bonusHealth;
    public int GetBonusDamage() => bonusDamage;
    public float GetBonusMoveSpeed() => bonusMoveSpeed;
    public float GetBonusAttackSpeed() => bonusAttackSpeed;
    public float GetBonusCrit() => bonusCrit;
}
