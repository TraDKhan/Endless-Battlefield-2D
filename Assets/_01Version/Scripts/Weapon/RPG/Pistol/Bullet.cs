using UnityEngine;
using static Weapon;

public class Bullet : MonoBehaviour
{
    private DamageContext damageContext;
    public float lifeTime = 5f;

    public void Init(DamageContext context)
    {
        damageContext = context;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        float finalDamage = damageContext.damage;

        if (Random.value < damageContext.critChance)
        {
            finalDamage *= damageContext.critMultiplier;
        }

        other.GetComponent<EnemyHealthController>()?.TakeDamage((int)(finalDamage));
        Destroy(gameObject);
    }
}
