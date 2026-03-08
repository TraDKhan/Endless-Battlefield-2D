using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance;
    [Header("Data")]
    [SerializeField] private WeaponData weaponData;

    public WeaponData Data => weaponData;
    public WeaponStats Stats { get; private set; }
    public WeaponStatSystem StatSystem { get; private set; }

    private WeaponBase weapon;
    public WeaponBase WeaponBase => weapon;

    private void Awake()
    {
        //todo: tối ưu singleton
        Instance = this;
        weapon = GetComponent<WeaponBase>();

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
    private void OnEnable()
    {
        if(EquipmentSystem.Instance != null) 
            EquipmentSystem.Instance.OnEquipped += OnEquipped;
    }
    private void OnEquipped(ItemInstance item)
    {
        Debug.Log("=== BEFORE EQUIP ===");
        LogAllStats();
        if (item.Data is IStatSource<WeaponStatType> source)
            StatSystem.AddSource(source);
        Debug.Log("=== AFTER EQUIP ===");
        LogAllStats();
    }
    private void LogAllStats()
    {
        foreach (WeaponStatType stat in System.Enum.GetValues(typeof(WeaponStatType)))
        {
            Debug.Log($"{stat}: {StatSystem.GetStat(stat)}");
        }
    }

}
