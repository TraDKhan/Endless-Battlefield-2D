using UnityEngine;

public class CharacterStatsController : MonoBehaviour
{
    public Player playerData;
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

        //tim
        if (level == null)
            level = FindAnyObjectByType<PlayerLevelSystem>();

        // Lắng nghe sự kiện stats đổi
        Stats.OnStatsChanged += ApplyStatsToHealth;

        // Khi lên cấp -> cần recal stats
        if (level != null)
            level.OnStatsBonusApplied += OnLevelUpBonusReceived;

        // Khởi tạo upgrade system
        if (upgrade != null)
            upgrade.Init(level, Stats);

        // Tính lần đầu
        Stats.RecalculateStats();
        ApplyStatsToHealth();
    }
    private void OnDestroy()
    {
        if (Stats != null)
            Stats.OnStatsChanged -= ApplyStatsToHealth;

        if (level != null)
            level.OnStatsBonusApplied -= OnLevelUpBonusReceived;
    }
    private void OnLevelUpBonusReceived(int hpBonus, int dmgBonus)
    {
        // Gọi RecalculateStats để tính lại stat mới
        Stats.RecalculateStats();
    }
    private void ApplyStatsToHealth()
    {
        if (healthController != null)
        {
            healthController.SetMaxHealth(Stats.GetMaxHealth());
            healthController.Heal(level.GetHealthBonus());
        }          
    }
}
