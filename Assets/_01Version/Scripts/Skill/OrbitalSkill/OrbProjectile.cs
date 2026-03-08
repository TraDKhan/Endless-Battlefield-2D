using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    private Transform owner;

    private float radius;
    private float speed;
    private float angle;
    private float damage;

    public void SetStats(Transform owner, float radius, float speed, float damage)
    {
        this.owner = owner;
        this.radius = radius;
        this.speed = speed;
        this.damage = damage;
    }

    public void SetAngle(float startAngle)
    {
        angle = startAngle;
    }

    void Update()
    {
        if (owner == null) return;

        angle += speed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(rad),
            Mathf.Sin(rad),
            0f
        ) * radius;

        transform.position = owner.position + offset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage((int)damage);
        }
    }
}