using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public CharacterStatSystem StatSystem { get; private set; }

    [Header("Data")]
    [SerializeField] private PlayerData playerData;

    [Header("Controllers")]
    [SerializeField] private PlayerMovementController movement;
    [SerializeField] private PlayerHealthController health;
    [SerializeField] private PlayerManaController mana;

    private bool initialized;

    public PlayerManaController Mana => mana;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        movement ??= GetComponent<PlayerMovementController>();
        health ??= GetComponent<PlayerHealthController>();
        mana ??= GetComponent<PlayerManaController>();

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
        mana.Initialize(this);
    }

    public void ApplyStats()
    {
        health.RefreshFromStats();
        mana.RefreshFromStats(true);
    }

    /// <summary>
    /// Gọi khi equip / buff / upgrade
    /// </summary>
    public void RecalculateStats()
    {
        if (!initialized) return;

        StatSystem.Recalculate();
        ApplyStats();
    }
}
