using UnityEngine;

public class CoinItem : PooledItem
{
    [SerializeField] private int value = 1;
    [SerializeField] private float pickupDistance = 0.5f;

    void Update()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            7f * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, player.position) < pickupDistance)
        {
            //player.GetComponent<PlayerCurrency>()?.AddCoin(value);
            DespawnSelf();
        }
    }
}
