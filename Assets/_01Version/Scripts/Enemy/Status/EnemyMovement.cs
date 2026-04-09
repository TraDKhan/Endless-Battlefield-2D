using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public float avoidDistance = 0.6f;
    public float sideAngle = 35f;
    public LayerMask obstacleLayer;

    Rigidbody2D rb;
    Vector2 cachedDir;
    float avoidTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTowards(Vector2 target, float speed)
    {
        Vector2 toTarget = (target - rb.position).normalized;

        FlipByDirection(toTarget.x);

        //Vector2 finalDir = GetBestDirection(toTarget);
        Vector2 finalDir;

        if (Vector2.Distance(rb.position, target) < 6f)
            finalDir = GetBestDirection(toTarget);
        else
            finalDir = toTarget;

        rb.linearVelocity = finalDir * speed;
    }

    Vector2 GetBestDirection(Vector2 desired)
    {
        if (avoidTimer > 0)
        {
            avoidTimer -= Time.deltaTime;
            return cachedDir;
        }

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
            {
                cachedDir = dir;
                avoidTimer = 0.1f; // giữ hướng 0.1s
                return dir;
            }
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
    void FlipByDirection(float xDir)
    {
        if (Mathf.Abs(xDir) < 0.01f) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(xDir) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
    }
}