using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIItemDetail : MonoBehaviour
{
    public static UIItemDetail Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text statsText;

    [Header("Buttons")]
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button removeButton;

    // =========================
    // STATE
    // =========================
    private InventorySlot inventorySlot;
    private ItemInstance itemInstance;
    private ItemData itemData;

    // =========================
    // UNITY
    // =========================
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        equipButton.onClick.AddListener(OnEquip);
        unequipButton.onClick.AddListener(OnUnequip);
        removeButton.onClick.AddListener(OnRemove);
    }

    // =========================
    // PUBLIC API
    // =========================

    /// <summary>
    /// Show item from inventory slot
    /// </summary>
    public void Show(InventorySlot slot)
    {
        if (slot == null || slot.Item == null) return;

        inventorySlot = slot;
        itemInstance = slot.Item;
        itemData = itemInstance.Data;

        BindUI();
        RefreshButtons();
    }

    /// <summary>
    /// Show item already equipped (no inventory slot)
    /// </summary>
    public void ShowEquipped(ItemInstance equippedItem)
    {
        if (equippedItem == null) return;

        inventorySlot = null;
        itemInstance = equippedItem;
        itemData = equippedItem.Data;

        BindUI();
        RefreshButtons();
    }
    public void Clear()
    {
        icon.enabled = false;
        nameText.text = string.Empty;
        descriptionText.text = string.Empty;
        statsText.text = string.Empty;

        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);

        inventorySlot = null;
        itemInstance = null;
        itemData = null;
        //gameObject.SetActive(false);
    }

    // =========================
    // UI
    // =========================
    private void BindUI()
    {
        if (itemData == null) return;

        icon.sprite = itemData.icon;
        icon.enabled = true;

        nameText.text = itemData.itemName;
        descriptionText.text = itemData.description;

        statsText.text = BuildStatText(itemData.stats);
    }

    private void RefreshButtons()
    {
        if (itemData == null || itemData.itemType != ItemType.Equipment)
        {
            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(false);
            return;
        }

        // Có slot inventory → chưa equip
        bool fromInventory = inventorySlot != null;

        equipButton.gameObject.SetActive(fromInventory);
        unequipButton.gameObject.SetActive(!fromInventory);
    }

    // =========================
    // BUTTON EVENTS
    // =========================
    private void OnEquip()
    {
        if (inventorySlot == null || itemInstance == null)
            return;

        bool success = PlayerController.Instance.TryEquip(itemInstance);
        if (!success)
            return;

        Clear();
    }

    private void OnUnequip()
    {
        if (itemInstance == null)
            return;

        PlayerController.Instance.TryUnequip(itemData.equipSlot);
        Clear();
    }

    private void OnRemove()
    {
        if (inventorySlot == null)
            return;

        UIRemoveItemPopup.Instance.Show(
            inventorySlot,
            InventorySystem.Instance // hoặc reference inject
        );
    }
    // =========================
    // HELPERS
    // =========================
    private string BuildStatText(List<StatEntry> stats)
    {
        if (stats == null || stats.Count == 0)
            return string.Empty;

        StringBuilder sb = new StringBuilder();
        foreach (var stat in stats)
            sb.AppendLine($"{stat.statType}: +{stat.value}");

        return sb.ToString();
    }
}
