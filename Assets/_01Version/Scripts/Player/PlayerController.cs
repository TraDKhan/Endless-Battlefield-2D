using UnityEngine;

[RequireComponent(
    typeof(PlayerMovementController),
    typeof(PlayerHealthController),
    typeof(PlayerManaController)
)]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    // =========================
    // CORE SYSTEM
    // =========================
    public StatSystem StatSystem { get; private set; }
    public CharacterStats CharacterStats { get; private set; }
    public EquipmentSystem Equipment { get; private set; }


    [Header("Data")]
    [SerializeField] private PlayerData playerData;

    [Header("Controllers")]
    [SerializeField] private PlayerMovementController movement;
    [SerializeField] private PlayerHealthController health;
    [SerializeField] private PlayerManaController mana;
    public PlayerManaController Mana => mana;


    // =========================
    // LIFECYCLE
    // =========================
    private void Awake()
    {
        // ---------- Singleton ----------
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ---------- Cache ----------
        movement ??= GetComponent<PlayerMovementController>();
        health ??= GetComponent<PlayerHealthController>();
        mana ??= GetComponent<PlayerManaController>();

        // ---------- Init Stat System ----------
        InitStatSystem();

        // ---------- Init Subsystems ----------
        Equipment = new EquipmentSystem(StatSystem);

        CharacterStats = new CharacterStats(StatSystem);

        // ---------- Init Controllers ----------
        movement.Initialize(this);
        health.Initialize(this);
        mana.Initialize(this);

        ApplyStats();
    }

    // =========================
    // STAT SYSTEM INIT
    // =========================
    private void InitStatSystem()
    {
        StatSystem = new StatSystem();

        // Base Character Stats
        foreach (var entry in playerData.baseStats)
        {
            StatSystem.SetBaseStat(
                StatContext.Character,
                entry.statType,
                entry.value
            );
        }

        var upgrade = FindAnyObjectByType<UpgradeSystem>();
        if (upgrade != null)
            StatSystem.AddSource(upgrade);

        StatSystem.Recalculate();
    }

    // =========================
    // APPLY
    // =========================
    public void ApplyStats()
    {
        health.RefreshFromStats();
        mana.RefreshFromStats(true);
    }
}
