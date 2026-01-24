using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private WeaponData weaponData;

    public WeaponStats Stats { get; private set; }
    public WeaponData Data => weaponData;

    private Weapon weapon;
    private StatSystem statSystem;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();

        // 1. Weapon có StatSystem RIÊNG
        statSystem = new StatSystem();

        // 2. Init base weapon stats
        InitBaseWeaponStats();

        // 3. Init stats wrapper
        Stats = new WeaponStats(statSystem);

        // 4. Init weapon logic
        weapon.Initialize(this);
    }

    private void Start()
    {
        if (!weaponData.IsValid(out var error))
        {
            Debug.LogError($"{weaponData.name}: {error}");
        }
    }

    // =========================
    // BASE STAT
    // =========================
    private void InitBaseWeaponStats()
    {
        foreach (var stat in weaponData.baseStats)
        {
            statSystem.SetBaseStat(
                StatContext.Weapon,
                stat.statType,
                stat.value
            );
        }

        statSystem.Recalculate();
    }

    // =========================
    // EXPOSE (Optional)
    // =========================
    public void AddStatSource(IStatSource source)
    {
        statSystem.AddSource(source);
        weapon.OnStatsChanged();
    }

    public void RemoveStatSource(IStatSource source)
    {
        statSystem.RemoveSource(source);
        weapon.OnStatsChanged();
    }
}
