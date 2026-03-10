using UnityEngine;

public class Item_Coin : PooledItem
{
    public int value = 1;

    protected override void OnCollected()
    {
        Debug.Log("Add coin: " + value);
        //CurrencySystem.Instance.AddCoin(value);
    }
}