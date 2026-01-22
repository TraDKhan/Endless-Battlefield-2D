using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private WeaponData weaponData;

    public WeaponStats Stats { get; private set; }

    private Weapon weapon;
    private WeaponUpgradeSystem upgradeSystem;

    public WeaponData Data => weaponData;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();

        // 1. Init stats
        Stats = new WeaponStats(weaponData);

        // 2. Bind upgrade system
        upgradeSystem = UpgradeSystem.Instance.Weapon;
        Stats.BindUpgradeSystem(upgradeSystem);

        // 3. Listen upgrade event
        upgradeSystem.OnWeaponStatsChanged += OnWeaponStatsChanged;

        // 4. First calculate
        Stats.Recalculate();

        weapon.Initialize(this);
    }

    private void Start()
    {
        if (!weaponData.IsValid(out var error))
        {
            Debug.LogError($"{weaponData.name}: {error}");
        }
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
