using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public float avoidDistance = 0.6f;
    public float sideAngle = 35f;
    public LayerMask obstacleLayer;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTowards(Vector2 target, float speed)
    {
        Vector2 toTarget = (target - rb.position).normalized;

        Vector2 finalDir = GetBestDirection(toTarget);

        rb.linearVelocity = finalDir * speed;
    }

    Vector2 GetBestDirection(Vector2 desired)
    {
        // Danh sách hướng kiểm tra
        Vector2[] dirs =
        {
            desired,
            Rotate(desired, sideAngle),
            Rotate(desired, -sideAngle),
            Rotate(desired, sideAngle * 2),
            Rotate(desired, -sideAngle * 2),
        };

        foreach (var dir in dirs)
        {
            if (!Physics2D.Raycast(rb.position, dir, avoidDistance, obstacleLayer))
                return dir;
        }

        // Bị kẹt → lùi nhẹ
        return -desired;
    }

    Vector2 Rotate(Vector2 v, float degree)
    {
        float rad = degree * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
        );
    }

    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
    }
}