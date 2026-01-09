using System;
using UnityEngine;

public class CharacterStatsController : MonoBehaviour
{
    public static CharacterStatsController Instance;
    public static event Action<CharacterStats> OnStatsReady;

    [Header("Data")]
    [SerializeField] private PlayerData playerData;

    [Header("Systems")]
    [SerializeField] private PlayerLevelSystem level;
    [SerializeField] private PlayerEquipmentController equip;
    [SerializeField] private PlayerBuffController buff;

    public CharacterStats Stats { get; private set; }

    private PlayerHealthController healthController;
    private UpgradeSystem upgradeSystem;

    private void Awake()
    {
        // ===== Singleton =====
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ===== Find systems =====
        if (level == null)
            level = FindAnyObjectByType<PlayerLevelSystem>();

        upgradeSystem = UpgradeSystem.Instance;
        healthController = GetComponent<PlayerHealthController>();

        // ===== Init stats =====
        Stats = new CharacterStats(
            playerData,
            level,
            equip,
            buff,
            upgradeSystem
        );

        // ===== Listen =====
        Stats.OnStatsChanged += ApplyStatsToHealth;
        OnStatsReady?.Invoke(Stats);

        // ===== First calc =====
        Stats.RecalculateStats();
        ApplyStatsToHealth();
    }

    private void OnDestroy()
    {
        if (Stats != null)
            Stats.OnStatsChanged -= ApplyStatsToHealth;

        if (Instance == this)
            Instance = null;
    }

    // =========================
    // APPLY TO HEALTH
    // =========================
    private void ApplyStatsToHealth()
    {
        if (healthController == null) return;

        int newMaxHp = Stats.GetMaxHealth();

        healthController.SetMaxHealth(newMaxHp);
    }
}
