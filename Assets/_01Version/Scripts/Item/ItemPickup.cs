using UnityEngine;

enum ItemType
{
    None,
    Heart,
    Exp
}
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
    [SerializeField] private int value;

    [Header("Settings")]
    [SerializeField] private float detectRadius = 3f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float pickupDistance = 0.5f;
    [SerializeField] private LayerMask playerLayer;

    private Transform player;

    void Update()
    {
        // Dùng OverlapCircle để phát hiện player trong bán kính
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, playerLayer);

        if (hit != null)
        {
            player = hit.transform;

            // vector hướng từ item tới player
            Vector2 dir = (player.position - transform.position).normalized;

            // di chuyển item về phía player
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );

            // nếu đã đủ gần thì nhặt
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= pickupDistance)
            {
                OnPickup();
            }
        }
    }

    void OnPickup()
    {
        if (itemType == ItemType.Heart)
        {
            PlayerHealthController playerHealthController = player.GetComponent<PlayerHealthController>();
            if (playerHealthController != null)
                playerHealthController.Heal(value);
        }                  
        else if(itemType == ItemType.Exp)
        {
            PlayerLevelSystem levelSystem = player.GetComponent<PlayerLevelSystem>();

            if (levelSystem != null)        
                levelSystem.AddEXP(value);
        }            

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}