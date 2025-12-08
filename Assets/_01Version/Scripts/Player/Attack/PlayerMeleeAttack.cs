using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour, IPlayerAttackController
{
    public float cooldown = 0.4f;
    public float attackRange = 0.7f;
    public LayerMask targetLayer;

    private float lastAttackTime = -99f;
    private CharacterStatsController statsCtrl;
    private PlayerAnimationController anim;
    private Transform attackPoint;

    void Awake()
    {
        statsCtrl = GetComponent<CharacterStatsController>();
        anim = GetComponent<PlayerAnimationController>();
        attackPoint = transform;
    }

    void Update()
    {
        AutoAttack();
    }

    private void AutoAttack()
    {
        // Nếu chưa tới cooldown → không attack
        if (!CanAttack()) return;

        // Kiểm tra xem có bất kỳ kẻ địch nào trong phạm vi
        Collider2D hit = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRange,
            targetLayer
        );

        if (hit != null && hit.TryGetComponent<IDamageable>(out var target))
        {
            // Tấn công kẻ địch đầu tiên phát hiện
            Attack(target);
        }
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + cooldown;
    }

    public void Attack(IDamageable target)
    {
        lastAttackTime = Time.time;

        anim.PlayAttack();

        target.TakeDamage(statsCtrl.Stats.totalDamage);

        Debug.Log("Tấn công kẻ địch: " + ((MonoBehaviour)target).name + statsCtrl.Stats.totalDamage + "ATK");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
