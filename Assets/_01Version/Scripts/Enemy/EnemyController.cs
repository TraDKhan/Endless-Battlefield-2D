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

    IEnemyState currentState;

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

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player not found");

        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        deadState = new EnemyDeadState(this);
    }
    
    void Start()
    {
        ChangeState(chaseState);
    }

    void Update()
    {
        currentState?.Update();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState == newState) return;
        Debug.Log(
            $"[EnemyFSM] {gameObject.name}: " +
            $"{currentState?.GetType().Name ?? "None"} → {newState.GetType().Name}",
            this
        );
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
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