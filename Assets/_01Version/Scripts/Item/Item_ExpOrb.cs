using UnityEngine;

public class Item_ExpOrb : PooledItem
{
    [SerializeField] int expAmount = 20;

    protected override void OnCollected()
    {
        ShowEXPPopup(expAmount);
        PlayerController.Instance?.LevelSystem?.AddEXP(expAmount);
    }

    private void ShowEXPPopup(int value)
    {
        if (PopupController.Instance == null) return;

        PopupController.Instance.ShowEXP(
            value,
            transform.position + Vector3.up * 0.5f
        );
    }
}
