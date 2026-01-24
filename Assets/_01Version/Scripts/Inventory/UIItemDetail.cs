using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemDetailContext
{
    Inventory,
    Equipped
}

public class UIItemDetail : MonoBehaviour
{
    public static UIItemDetail Instance { get; private set; }

    [Header("UI")]
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text statsText;

    [Header("Buttons")]
    public Button equipButton;
    public Button unequipButton;

    private InventorySlot inventoryItem;
    private ItemData itemData;
    private ItemDetailContext context;

    void Awake()
    {
        Instance = this;

        equipButton.onClick.AddListener(OnEquip);
        unequipButton.onClick.AddListener(OnUnequip);
    }

    // =========================
    // SHOW FROM INVENTORY
    // =========================
    public void Show(InventorySlot item)
    {
        context = ItemDetailContext.Inventory;
        inventoryItem = item;
        itemData = item.data;

        BindUI(itemData);
        RefreshButtons();
    }

    // =========================
    // SHOW FROM HERO SLOT
    // =========================
    public void ShowEquipped(ItemData data)
    {
        context = ItemDetailContext.Equipped;
        itemData = data;
        inventoryItem = null;

        BindUI(itemData);
        RefreshButtons();
    }

    void BindUI(ItemData data)
    {
        icon.sprite = data.icon;
        nameText.text = data.itemName;
        descriptionText.text = data.description;
        statsText.text = BuildStatText(data.stats);
    }

    string BuildStatText(List<StatEntry> stats)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var stat in stats)
            sb.AppendLine($"{stat.statType}: +{stat.value}");
        return sb.ToString();
    }

    void RefreshButtons()
    {
        bool isEquipment = itemData.itemType == ItemType.Equipment;

        equipButton.gameObject.SetActive(
            isEquipment && context == ItemDetailContext.Inventory
        );

        unequipButton.gameObject.SetActive(
            isEquipment && context == ItemDetailContext.Equipped
        );
    }

    public void OnEquip()
    {
        if (inventoryItem == null) return;
        if (itemData.itemType != ItemType.Equipment) return;

        PlayerController.Instance.Equipment.Equip(itemData);
    }

    public void OnUnequip()
    {
        PlayerController.Instance.Equipment.Unequip(itemData.equipmentSlot);
    }
}
