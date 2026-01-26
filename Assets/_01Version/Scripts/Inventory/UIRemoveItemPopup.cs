using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRemoveItemPopup : MonoBehaviour
{
    public static UIRemoveItemPopup Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_InputField amountInput;

    [Header("Buttons")]
    [SerializeField] private Button removeAllButton;
    [SerializeField] private Button removeOneButton;
    [SerializeField] private Button removeAmountButton;
    [SerializeField] private Button cancelButton;

    private InventorySlot slot;
    private InventorySystem inventory;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        removeAllButton.onClick.AddListener(RemoveAll);
        removeOneButton.onClick.AddListener(RemoveOne);
        removeAmountButton.onClick.AddListener(RemoveAmount);
        cancelButton.onClick.AddListener(Hide);
    }

    public void Show(InventorySlot slot, InventorySystem inventory)
    {
        if (slot == null || slot.Item == null)
            return;

        this.slot = slot;
        this.inventory = inventory;

        bool isStackable = slot.Item.IsStackable;

        removeOneButton.gameObject.SetActive(isStackable);
        removeAmountButton.gameObject.SetActive(isStackable);

        titleText.text = $"Remove {slot.Item.Data.itemName}";
        amountInput.text = "1";

        gameObject.SetActive(true);
    }

    private void Hide()
    {
        slot = null;
        inventory = null;
        gameObject.SetActive(false);
    }

    // =========================
    // ACTIONS
    // =========================
    private void RemoveAll()
    {
        inventory.RemoveSlot(slot);
        Finish();
    }

    private void RemoveOne()
    {
        inventory.RemoveFromSlot(slot, 1);
        Finish();
    }

    private void RemoveAmount()
    {
        if (!int.TryParse(amountInput.text, out int amount))
            return;

        amount = Mathf.Clamp(amount, 1, slot.Item.quantity);
        inventory.RemoveFromSlot(slot, amount);
        Finish();
    }

    private void Finish()
    {
        Hide();
        UIItemDetail.Instance.Clear();
    }
}
