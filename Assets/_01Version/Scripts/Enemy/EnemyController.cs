using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHealthController), typeof(EnemyAnimationController), typeof(EnemyMovement))]
[RequireComponent (typeof(PoolIdentity))]
public class EnemyController : MonoBehaviour, IPoolable
{
    [Header("Pool")]
    public PoolIdentity Identity { get; set; }

    [Header("Stats")]
    public EnemyStats stats;

    [Header("Components")]
    //public EnemyBase enemyBase;
    public EnemyMovement movement;
    public EnemyAnimationController anim;

    private EnemyHealthController health;
    private Rigidbody2D rb;

    public Transform target;

    #region State Machine
    private EnemyContext context;
    private IEnemyState currentState;

    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public EnemyDeadState deadState;
    #endregion

    private bool isAlive;
    public event Action<EnemyController> OnEnemyDead;

    void Awake()
    {
        health = GetComponent<EnemyHealthController>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<EnemyMovement>();
        anim = GetComponent<EnemyAnimationController>();

        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;

        context = new EnemyContext(this);

        //
        InitAttacks();

        idleState = new EnemyIdleState(context);
        chaseState = new EnemyChaseState(context);
        attackState = new EnemyAttackState(context);
        deadState = new EnemyDeadState(context);

        health.OnDeath += HandleDeath;
    }

    void Update()
    {
        currentState?.Update();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    #region POOLABLE
    public void OnSpawn()
    {
        isAlive = true;

        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        health.Init(stats.maxHealth, context);
        rb.linearVelocity = Vector2.zero;

        currentState = null;
        ChangeState(EnemyStateID.Chase);
    }

    public void OnDespawn()
    {
        isAlive = false;

        currentState?.Exit();
        currentState = null;

        StopAllCoroutines();
        ClearRuntimeEvents();
    }

    public void Despawn()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }
    #endregion

    #region STATE CONTROL
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

        currentState?.Exit();

        currentState = newState;

        currentState.Enter();
    }
    #endregion

    #region COMBAT HELPERS
    public bool IsInAttackRange()
    {
        if (target == null) return false;
        return Vector2.Distance(transform.position, target.position) <= stats.attackRange;
    }

    public void FaceTarget(Vector2 targetPos)
    {
        Vector2 dir = targetPos - (Vector2)transform.position;

        if (Mathf.Abs(dir.x) < 0.01f) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    #endregion

    #region Death
    private void HandleDeath()
    {
        if (!isAlive) return;

        isAlive = false;
        OnEnemyDead?.Invoke(this);

        ChangeState(EnemyStateID.Dead);
    }
    #endregion

    #region Init Helpers
    private void InitAttacks()
    {
        var melee = GetComponent<EnemyMeleeAttack>();
        if (melee != null) melee.Init(context);

        //var charge = GetComponent<EnemyChargeAttack>();
        //if (charge != null) charge.Init(context);

        //var contact = GetComponent<EnemyContactAttack>();
        //if (contact != null) contact.Init(context);

        var ranged = GetComponent<EnemyRangedAttack>();
        if (ranged != null) ranged.Init(context);
    }

    private void ClearRuntimeEvents()
    {
        // clear delegate runtime nếu có
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (stats == null) return;

        // Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stats.personalSpace);
    }

    private void Start()
    {
        OnSpawn();
    }
}