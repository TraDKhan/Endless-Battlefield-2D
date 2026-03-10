using UnityEngine;

public class PlayerPickupMagnet : MonoBehaviour
{
    public float scanRadius = 5f;
    public LayerMask itemLayer;

    public float scanInterval = 0.2f;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= scanInterval)
        {
            ScanItems();
            timer = 0;
        }
    }

    void ScanItems()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            scanRadius,
            itemLayer
        );

        foreach (var hit in hits)
        {
            PooledItem item = hit.GetComponent<PooledItem>();

            if (item != null)
            {
                item.StartMagnet(transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRadius);
    }
}