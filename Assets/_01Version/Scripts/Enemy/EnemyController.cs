using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public EnemyStats stats;

    [Header("Components")]
    public EnemyHealthController health;
    public EnemyMovement movement;
    public EnemySensor sensor;

    [HideInInspector] public Transform player;

    IEnemyState currentState;

    // States
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    //public EnemyDeadState deadState;

    void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        sensor = GetComponent<EnemySensor>();
        health = GetComponent<EnemyHealthController>();

        health.OnDeath += HandleDeath;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player not found");

        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        //deadState = new EnemyDeadState(this);
    }

    private void OnDestroy()
    {
        health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        ChangeState(new EnemyDeadState(this));
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
}