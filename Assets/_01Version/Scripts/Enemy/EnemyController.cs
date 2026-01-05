using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public EnemyStats stats;

    [Header("Components")]
    public EnemyBase enemyBase;
    public EnemyMovement movement;
    public EnemySensor sensor;
    public EnemyAnimationController anim;

    [HideInInspector] public Transform player;

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
        sensor = GetComponent<EnemySensor>();
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
        ChangeState(idleState);
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

    public bool IsPlayerDetected()
    {
        return sensor.IsPlayerInRange(stats.detectRange);
    }

    public bool IsInAttackRange()
    {
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

        // Detect Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.detectRange);

        // Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);

        // Nếu là melee enemy
        if (stats.enemyType == EnemyAttackType.Melee)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, stats.meleeStopRange);
        }
    }

}