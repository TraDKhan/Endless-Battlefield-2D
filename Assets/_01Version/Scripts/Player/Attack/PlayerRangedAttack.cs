using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour, IPlayerAttackController
{
    public float cooldown = 0.5f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackRange = 5f;
    public LayerMask targetLayer;

    private float lastAttackTime = -99f;
    private CharacterStatsController statsCtrl;
    private PlayerAnimationController anim;

    void Awake()
    {
        statsCtrl = GetComponent<CharacterStatsController>();
        anim = GetComponent<PlayerAnimationController>();
    }

    void Update()
    {
        AutoAttack();
    }

    private void AutoAttack()
    {
        if (!CanAttack()) return;

        // Lấy tất cả kẻ địch trong phạm vi
        Collider2D[] hits = Physics2D.OverlapCircleAll(firePoint.position, attackRange, targetLayer);

        if (hits.Length == 0) return;

        // Tìm kẻ địch gần nhất
        float minDist = float.MaxValue;
        IDamageable nearestEnemy = null;

        foreach (var h in hits)
        {
            if (h.TryGetComponent<IDamageable>(out var target))
            {
                float dist = Vector2.Distance(firePoint.position, h.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestEnemy = target;
                }
            }
        }

        if (nearestEnemy != null)
        {
            Attack(nearestEnemy);
        }
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + cooldown;
    }

    public void Attack(IDamageable target)
    {
        if (!CanAttack() || statsCtrl == null || statsCtrl.Stats == null || target == null) return;

        lastAttackTime = Time.time;

        //anim?.PlayAttack();

        // Tạo projectile
        var obj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        var proj = obj.GetComponent<Projectile>();

        if (proj != null)
        {
            //proj.damage = statsCtrl.Stats.totalDamage;
            //proj.SetTarget(((MonoBehaviour)target).transform); // giả sử projectile có logic SetTarget
        }

        Debug.Log("Bắn projectile vào: " + ((MonoBehaviour)target).name);
    }

    void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }
}
