using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public CharacterStatSystem StatSystem { get; private set; }
    public PlayerLevelSystem LevelSystem { get; private set; }

    [Header("Data")]
    [SerializeField] private PlayerData playerData;

    [Header("Controllers")]
    [SerializeField] private PlayerMovementController movement;
    [SerializeField] private PlayerHealthController health;
    [SerializeField] private PlayerAnimationController anim;

    public PlayerAnimationController Anim => anim;
    public PlayerHealthController Health => health;

    private bool initialized;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LevelSystem = new PlayerLevelSystem();

        movement ??= GetComponent<PlayerMovementController>();
        health ??= GetComponent<PlayerHealthController>();
        anim ??= GetComponent<PlayerAnimationController>();

        InitializeStats();          //Stats trước
        InitializeControllers();    //Controller sau

        ApplyStats();               //Sync cuối
        initialized = true;
    }

    private void InitializeStats()
    {
        StatSystem = new CharacterStatSystem();

        foreach (var entry in playerData.baseStats)
        {
            StatSystem.SetBaseStat(entry.statType, entry.value);
        }

        StatSystem.Recalculate();
    }

    private void InitializeControllers()
    {
        movement.Initialize(this);
        health.Initialize(this);
    }

    public void ApplyStats()
    {
        health.RefreshFromStats();
    }

    /// <summary>
    /// Gọi khi equip / buff / upgrade
    /// </summary>
    /// 
    private void OnEnable()
    {
        if (EquipmentSystem.Instance != null)
        {
            EquipmentSystem.Instance.OnEquipped += OnEquipped;
            EquipmentSystem.Instance.OnUnequipped += OnUnequipped;
        }
    }

    private void OnEquipped(ItemInstance item)
    {
        Debug.Log("=== BEFORE EQUIP ===");
        LogAllStats();
        if (item.Data is IStatSource<CharacterStatType> source)
            StatSystem.AddSource(source);
        Debug.Log("=== AFTER EQUIP ===");
        LogAllStats();
    }

    private void OnUnequipped(ItemInstance item)
    {
        if (item.Data is IStatSource<CharacterStatType> source)
            StatSystem.RemoveSource(source);
    }


    public void RecalculateStats()
    {
        if (!initialized) return;

        StatSystem.Recalculate();
        ApplyStats();
    }

    private void LogAllStats()
    {
        foreach (CharacterStatType stat in System.Enum.GetValues(typeof(CharacterStatType)))
        {
            Debug.Log($"{stat}: {StatSystem.GetStat(stat)}");
        }
    }
    public void OnDes()
    {
        Time.timeScale = 0f;
        Destroy(gameObject);
    }
}
