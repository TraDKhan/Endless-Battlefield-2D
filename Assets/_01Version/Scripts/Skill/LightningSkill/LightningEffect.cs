using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    private int damage;
    private LayerMask enemyLayer;

    public void Init(int damage, LayerMask enemyLayer)
    {
        this.damage = damage;
        this.enemyLayer = enemyLayer;
    }

    // GỌI TỪ ANIMATION EVENT
    public void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.6f, enemyLayer);

        foreach (var hit in hits)
        {
            var hp = hit.GetComponent<EnemyHealthController>();
            if (hp != null)
                hp.TakeDamage(damage);
        }
    }

    // GỌI TỪ ANIMATION EVENT CUỐI
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
