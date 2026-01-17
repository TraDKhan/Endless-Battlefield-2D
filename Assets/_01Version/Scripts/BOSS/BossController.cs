using System;
using UnityEngine;

[RequireComponent(typeof(BossPhaseController))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAnimationController))]
[RequireComponent(typeof(EnemyHealthController))]

public class BossController : MonoBehaviour
{
    public EnemyStats stats;

    [Header("Components")]
    public BossPhaseController phaseController;
    public EnemyMovement movement;
    public EnemyAnimationController anim;
    [SerializeField] private EnemyHealthController health;

    [Header("Runtime")]
    public Transform player;
    public LayerMask targetLayer;
    public int CurrentPhase => phaseController.CurrentPhase;
    public EnemyHealthController Health => health;
    public bool IsCastingSkill { get; private set; }
    public event System.Action<BossController> OnBossDead;

    void Awake()
    {
        phaseController = GetComponent<BossPhaseController>();
        movement = GetComponent<EnemyMovement>();
        anim = GetComponent<EnemyAnimationController>();
        health = GetComponent<EnemyHealthController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        health.Init(stats.maxHealth);
        health.OnDeath += HandleDeath;

        BossUIManager.Instance.RegisterBoss(this);
    }

    void Update()
    {
        if (!player) return;

        if (!IsCastingSkill)
        {
            HandleMovement();
        }

        FaceTarget(player.position);
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnDeath -= HandleDeath;
        BossUIManager.Instance.UnregisterBoss(this);
    }
    void HandleDeath()
    {
        // 1️⃣ Báo cho Spawner
        OnBossDead?.Invoke(this);

        // 2️⃣ Gỡ UI
        BossUIManager.Instance.UnregisterBoss(this);

        // 3️⃣ Hủy boss
        Destroy(gameObject);
    }
    void HandleMovement()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        bool isMoving = false;

        if (distance > stats.attackRange)
        {
            movement.MoveTowards(player.position, stats.moveSpeed);
            isMoving = true;
        }
        else if (distance > stats.personalSpace)
        {
            movement.Stop();
            isMoving = false;
        }
        else
        {
            Vector2 retreatTarget =
                (Vector2)transform.position +
                ((Vector2)transform.position - (Vector2)player.position).normalized *
                stats.personalSpace;

            movement.MoveTowards(retreatTarget, stats.moveSpeed * 0.5f);
            isMoving = true;
        }

        anim?.SetMoving(isMoving);
    }

    public void FaceTarget(Vector2 targetPos)
    {
        Vector2 dir = targetPos - (Vector2)transform.position;

        if (Mathf.Abs(dir.x) < 0.01f) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void SetCastingSkill(bool value)
    {
        IsCastingSkill = value;

        if (value)
        {
            movement.Stop();
            anim?.SetMoving(false);
        }
    }

    private void OnDrawGizmos()
    {
        if (stats == null) return;

        // Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stats.personalSpace);
    }
}
