using UnityEngine;

public class EnemyContactAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private EnemyAnimationController anim;
    [SerializeField] private LayerMask targetLayer;

    private bool isAttacking;
    private bool targetInRange;
    private IDamageable currentTarget;

    private float lastDamageTime;

    // Có thể bắt đầu 1 đợt attack mới?
    public bool CanAttack => Time.time >= lastDamageTime + stats.attackCooldown;

    public float Cooldown => stats.attackCooldown;

    #region IEnemyAttack

    public void StartAttack()
    {
        isAttacking = true;
        if(anim != null) 
            anim.PlayAttack();
    }

    public void UpdateAttack()
    {
        if (!isAttacking) return;
        if (!targetInRange) return;
        if (currentTarget == null) return;

        if (Time.time < lastDamageTime + stats.attackCooldown)
            return;

        lastDamageTime = Time.time;
        currentTarget.TakeDamage(stats.damage);

    }

    public void StopAttack()
    {
        isAttacking = false;
    }

    #endregion

    #region Trigger Detection ONLY
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) == 0)
            return;

        currentTarget = other.GetComponent<IDamageable>();
        if (currentTarget != null)
        {
            targetInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) == 0)
            return;

        if (other.GetComponent<IDamageable>() == currentTarget)
        {
            targetInRange = false;
            currentTarget = null;
        }
    }
    #endregion
}
