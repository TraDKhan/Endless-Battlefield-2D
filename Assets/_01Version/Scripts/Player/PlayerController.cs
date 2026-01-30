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
    public EquipmentSystem EquipSystem { get; private set; }

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

        // ---------- Init Equip system ----------
        InitEquipSystem();

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
    private void InitEquipSystem()
    {
        EquipSystem = new EquipmentSystem(
            StatSystem,
            new[]
            {
            EquipmentSlotType.Helmet,
            EquipmentSlotType.Armor,
            EquipmentSlotType.Weapon,
            EquipmentSlotType.Ring,
            EquipmentSlotType.Amulet
            }
        );

        // Khi equip đổi → stat đổi → apply lại
        EquipSystem.OnEquipmentChanged += ApplyStats;
    }

    // =========================
    // APPLY
    // =========================
    public void ApplyStats()
    {
        health.RefreshFromStats();
        mana.RefreshFromStats(true);
    }
    //
    public bool TryEquip(ItemInstance item)
    {
        if (item == null || !item.IsEquipment)
            return false;

        // 1️⃣ Equip
        ItemInstance oldItem = EquipSystem.Equip(item);

        // Nếu equip thất bại
        if (EquipSystem.GetEquippedItem(item.Data.equipSlot) != item)
            return false;

        // 2️⃣ Remove item khỏi inventory
        InventorySystem.Instance.RemoveExact(item);

        // 3️⃣ Trả item cũ về inventory
        if (oldItem != null)
            InventorySystem.Instance.AddItem(oldItem);

        return true;
    }

    public bool TryUnequip(EquipmentSlotType slotType)
    {
        ItemInstance item = EquipSystem.Unequip(slotType);
        if (item == null)
            return false;

        InventorySystem.Instance.AddItem(item);
        return true;
    }

}
