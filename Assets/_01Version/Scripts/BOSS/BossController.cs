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
    [SerializeField] private UIBossHealth bossUI;

    [Header("Runtime")]
    public Transform player;
    public LayerMask targetLayer;
    public int CurrentPhase => phaseController.CurrentPhase;
    public bool IsCastingSkill { get; private set; }

    void Awake()
    {
        phaseController = GetComponent<BossPhaseController>();
        movement = GetComponent<EnemyMovement>();
        anim = GetComponent<EnemyAnimationController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        bossUI.Bind(health);
        health.OnDeath += () => bossUI.gameObject.SetActive(false);
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
