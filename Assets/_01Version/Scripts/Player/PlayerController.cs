using System;
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
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerAnimation anim;

    [Header("UI Status")]
    [SerializeField] private UIStatus uiStatus;

    public PlayerAnimation Anim => anim;
    public PlayerHealth Health => health;

    private bool initialized;
    public static event Action<PlayerController> OnPlayerReady;

    private void Awake()
    {
        //if (Instance != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        Instance = this;

        LevelSystem = new PlayerLevelSystem();

        movement ??= GetComponent<PlayerMovement>();
        health ??= GetComponent<PlayerHealth>();
        anim ??= GetComponent<PlayerAnimation>();

        InitializeStats();          //Stats trước
        InitializeControllers();    //Controller sau

        ApplyStats();               //Sync cuối
        initialized = true;
    }

    private void Start()
    {
        OnPlayerReady?.Invoke(this);

        uiStatus ??= FindFirstObjectByType<UIStatus>();
        uiStatus.Init(LevelSystem, health);
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
        LogAllStats();
        if (item.Data is IStatSource<CharacterStatType> source)
            StatSystem.AddSource(source);
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
        }
    }

    //gọi qua Anim
    //to do: đang bị xung đột với respawn hồi sinh
    //cần để thời gian delay cao hơn để tránh xung đột
    public void OnDes()
    {
        gameObject.SetActive(false);
    }

    // Thêm vào trong PlayerController.cs

    public void HandleRespawn(Vector3 respawnPosition)
    {
        // 1. Chuyển vị trí
        transform.position = respawnPosition;

        // 2. Reset trạng thái Health
        health.Respawn();

        // 3. Reset Animation
        //anim?.PlayIdle(); // Giả sử bạn có hàm PlayIdle hoặc Trigger tương ứng

        // 4. Kích hoạt lại các hệ thống (nếu bạn đã tắt chúng ở Die)
        movement.enabled = true;
    }

    [ContextMenu("Test Add EXP")]

    private void TestAddEXP()
    {
        LevelSystem.AddEXP(150);
    }
}
