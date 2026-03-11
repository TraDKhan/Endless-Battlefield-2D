using UnityEngine;

public class Item_Coin : PooledItem
{
    [SerializeField] private int value = 1;

    protected override void OnCollected()
    {
        if (CurrencyManager.Instance == null) return;

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(value);

            PopupController.Instance?.ShowCoin(
                value,
                transform.position + Vector3.up * 0.5f
            );
        }
    }
}