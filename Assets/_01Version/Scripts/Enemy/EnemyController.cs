using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public EnemyStats stats;

    [Header("Components")]
    public EnemyBase enemyBase;
    public EnemyMovement movement;
    public EnemyAnimationController anim;

    public Transform player;
    public LayerMask targetLayer;

    private EnemyContext context;
    private IEnemyState currentState;

    // States
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public EnemyDeadState deadState;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        movement = GetComponent<EnemyMovement>();
        anim = GetComponent<EnemyAnimationController>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        context = new EnemyContext(this);
        enemyBase.OnEnemyDead += HandleEnemyDead;

        //
        var melee = GetComponent<EnemyMeleeAttack>();
        if (melee != null)
            melee.Init(context);

        var charge = GetComponent<EnemyChargeAttack>();
        if (charge != null)
            charge.Init(context);

        var contact = GetComponent<EnemyContactAttack>();
        if (contact != null)
            contact.Init(context);

        var ranged = GetComponent<EnemyRangedAttack>();
        if (ranged != null)
            ranged.Init(context);

        idleState = new EnemyIdleState(context);
        chaseState = new EnemyChaseState(context);
        attackState = new EnemyAttackState(context);
        deadState = new EnemyDeadState(context);
    }

    void Start()
    {
        ChangeState(EnemyStateID.Chase);
    }

    void Update()
    {
        currentState?.Update();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void ChangeState(EnemyStateID id)
    {
        switch (id)
        {
            case EnemyStateID.Idle:
                ChangeState(idleState);
                break;

            case EnemyStateID.Chase:
                ChangeState(chaseState);
                break;

            case EnemyStateID.Attack:
                ChangeState(attackState);
                break;

            case EnemyStateID.Dead:
                ChangeState(deadState);
                break;
        }
    }
    void ChangeState(IEnemyState newState)
    {
        if (currentState == newState) return;

        Debug.Log($"Exiting state: {currentState?.GetType().Name}");
        currentState?.Exit();

        currentState = newState;

        Debug.Log($"Entering state: {currentState.GetType().Name}");
        currentState.Enter();
    }
    public bool IsInAttackRange()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) <= stats.attackRange;
    }

    public void FaceTarget(Vector2 targetPos)
    {
        Vector2 dir = targetPos - (Vector2)transform.position;

        if (Mathf.Abs(dir.x) < 0.01f) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
    void HandleEnemyDead(EnemyBase enemy)
    {
        ChangeState(EnemyStateID.Dead);
    }
    public void Despawn()
    {
        ObjectPoolManager.Instance.Despawn(enemyBase);
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