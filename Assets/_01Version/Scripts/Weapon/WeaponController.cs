using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private WeaponData weaponData;

    public WeaponData Data => weaponData;
    public WeaponStats Stats { get; private set; }
    public WeaponStatSystem StatSystem { get; private set; }

    private Weapon weapon;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();

        StatSystem = new WeaponStatSystem();

        InitBaseStats();

        Stats = new WeaponStats(StatSystem);

        weapon.Initialize(this);
    }
    private void InitBaseStats()
    {
        foreach (var entry in weaponData.baseStats)
        {
            StatSystem.SetBaseStat(entry.statType, entry.value);
        }
    }

    public void AddStatSource(IStatSource<WeaponStatType> source)
    {
        StatSystem.AddSource(source);
        weapon.OnStatsChanged();
    }

    public void RemoveStatSource(IStatSource<WeaponStatType> source)
    {
        StatSystem.RemoveSource(source);
        weapon.OnStatsChanged();
    }
}
