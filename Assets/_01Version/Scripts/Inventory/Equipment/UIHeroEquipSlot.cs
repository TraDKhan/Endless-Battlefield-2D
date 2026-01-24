using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHeroEquipSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EquipmentSlot slotType;
    [SerializeField] private Image icon;

    private void OnEnable()
    {
        TryBind();
    }

    private void OnDisable()
    {
        if (PlayerController.Instance?.Equipment != null)
            PlayerController.Instance.Equipment.OnEquipmentChanged -= Refresh;
    }

    void TryBind()
    {
        if (PlayerController.Instance == null ||
            PlayerController.Instance.Equipment == null)
        {
            Invoke(nameof(TryBind), 0.05f);
            return;
        }

        PlayerController.Instance.Equipment.OnEquipmentChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        var item = PlayerController.Instance.Equipment.GetItem(slotType);

        icon.enabled = item != null;
        icon.sprite = item ? item.icon : null;
    }

    // =========================
    // CLICK HERO SLOT
    // =========================
    public void OnPointerClick(PointerEventData eventData)
    {
        var item = PlayerController.Instance.Equipment.GetItem(slotType);
        if (item == null) return;

        UIItemDetail.Instance.ShowEquipped(item);
    }
}
