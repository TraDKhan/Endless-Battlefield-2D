using UnityEngine;

public class CharacterStatsController : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerLevelSystem level;
    public PlayerEquipmentController equip;
    public PlayerBuffController buff;

    public CharacterStats Stats { get; private set; }
    private PlayerHealthController healthController;

    void Awake()
    {
        var upgrade = FindAnyObjectByType<PlayerUpgradeSystem>();

        Stats = new CharacterStats(playerData, level, equip, buff, upgrade);
        healthController = GetComponent<PlayerHealthController>();

        if (level == null)
            level = FindAnyObjectByType<PlayerLevelSystem>();

        Stats.OnStatsChanged += ApplyStatsToHealth;

        if (level != null)
            level.OnLevelUp += OnLevelUpBonusReceived;

        if (upgrade != null)
            upgrade.Init(level, Stats);

        Stats.RecalculateStats();
        ApplyStatsToHealth();
    }

    private void OnDestroy()
    {
        if (Stats != null)
            Stats.OnStatsChanged -= ApplyStatsToHealth;

        if (level != null)
            level.OnLevelUp -= OnLevelUpBonusReceived;
    }

    private void OnLevelUpBonusReceived(int hpBonus)
    {
        Stats.RecalculateStats();
    }

    private void ApplyStatsToHealth()
    {
        if (healthController != null)
        {
            healthController.SetMaxHealth(Stats.GetMaxHealth());
        }
    }
}
