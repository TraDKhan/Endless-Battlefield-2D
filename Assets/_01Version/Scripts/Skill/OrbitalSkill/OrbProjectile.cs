using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    private Transform owner;
    private float radius;
    private float speed;
    private float angle;
    private float damage;

    public void Init(Transform owner, float radius, float speed, float startAngle, float damage)
    {
        this.owner = owner;
        this.radius = radius;
        this.speed = speed;
        this.angle = startAngle;
        this.damage = damage;
    }

    void Update()
    {
        if (owner == null) return;

        angle += speed * Time.deltaTime;
        float rad = angle;// * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(rad),
            Mathf.Sin(rad),
            0
        ) * radius;

        transform.position = owner.position + offset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Debug.Log("trigger");
        if (other.TryGetComponent(out IDamageable dmg))
            dmg.TakeDamage((int)damage);
    }
}
