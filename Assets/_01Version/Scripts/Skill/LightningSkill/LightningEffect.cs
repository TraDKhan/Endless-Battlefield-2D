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
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            0.6f,
            enemyLayer
        );

        if (hit == null) return;

        var hp = hit.GetComponent<EnemyHealthController>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
        }
    }

    // GỌI TỪ ANIMATION EVENT CUỐI
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
