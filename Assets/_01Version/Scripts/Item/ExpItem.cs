using UnityEngine;

public class ExpItem : PooledItem
{
    [SerializeField] private int expAmount = 5;
    [SerializeField] private float pickupDistance = 0.5f;

    void Update()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            6f * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, player.position) < pickupDistance)
        {
            player.GetComponent<PlayerLevelSystem>()?.AddEXP(expAmount);
            DespawnSelf();
        }
    }
}
