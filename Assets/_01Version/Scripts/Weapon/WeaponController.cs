using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private WeaponData weaponData;

    [Header("Systems")]
    [SerializeField] private WeaponUpgradeSystem upgradeSystem;

    public WeaponStats Stats { get; private set; }

    private Weapon weapon;

    public WeaponData Data => weaponData;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();

        if (upgradeSystem == null)
            upgradeSystem = FindAnyObjectByType<WeaponUpgradeSystem>();

        // ===== Init Stats =====
        Stats = new WeaponStats(weaponData);
        Stats.BindUpgradeSystem(upgradeSystem);

        // ===== Inject =====
        weapon.Initialize(this);

        // ===== Listen =====
        if (upgradeSystem != null)
            upgradeSystem.OnWeaponStatsChanged += OnWeaponStatsChanged;

        Stats.Recalculate();
    }

    private void OnDestroy()
    {
        if (upgradeSystem != null)
            upgradeSystem.OnWeaponStatsChanged -= OnWeaponStatsChanged;
    }

    private void OnWeaponStatsChanged()
    {
        Stats.Recalculate();
        weapon.OnStatsChanged();
    }
}
