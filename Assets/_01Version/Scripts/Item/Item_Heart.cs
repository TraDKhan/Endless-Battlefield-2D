
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Item_Heart : PooledItem
{
    public int healAmount = 10;

    protected override void OnCollected()
    {
        Debug.Log("Add coin: " + healAmount);
        //PlayerHealthSystem.Instance.Heal(healAmount);
    }
}