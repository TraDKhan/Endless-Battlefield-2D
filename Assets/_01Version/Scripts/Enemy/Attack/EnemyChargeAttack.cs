using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChargeAttack : MonoBehaviour, IEnemyAttack
{
    [Header("References")]
    [SerializeField] private EnemyStats stats;
    [SerializeField] private Transform target;
    [SerializeField] private EnemyAnimationController anim;

    [Header("Charge Settings")]
    [SerializeField] private float windUpTime = 0.4f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.6f;
    [SerializeField] private LayerMask targetLayer;

    [Header("Retreat Settings")]
    [SerializeField] private float retreatDistance = 2.5f;
    [SerializeField] private float retreatSpeed = 4f;
    [SerializeField] private float retreatDuration = 0.6f;

    private Rigidbody2D rb;
    private float lastAttackTime;
    private float timer;

    private Vector2 dashDirection;
    private Vector2 retreatDirection;
    private Vector2 dashStartPosition;

    private enum State
    {
        Idle,
        WindUp,
        Dashing,
        Retreating,
        Cooldown
    }

    private State currentState = State.Idle;
    private void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
            else
                Debug.LogError("Player not found");
        }
    }
    #region IEnemyAttack

    public bool CanAttack =>
        currentState == State.Idle &&
        Time.time >= lastAttackTime + stats.attackCooldown;

    public float Cooldown => stats.attackCooldown;

    public void StartAttack()
    {
        if (!target) return;
        Debug.Log("tấn công");

        currentState = State.WindUp;
        timer = windUpTime;

        dashDirection = (target.position - transform.position).normalized;
        dashStartPosition = rb.position;

        rb.linearVelocity = Vector2.zero;
        // anim?.PlayChargeWindUp();
    }

    public void UpdateAttack()
    {
        switch (currentState)
        {
            case State.WindUp:
                UpdateWindUp();
                break;

            case State.Dashing:
                UpdateDash();
                break;

            case State.Retreating:
                UpdateRetreat();
                break;

            case State.Cooldown:
                UpdateCooldown();
                break;
        }
    }

    public void StopAttack()
    {
        rb.linearVelocity = Vector2.zero;
        currentState = State.Cooldown;
        lastAttackTime = Time.time;
    }

    #endregion

    #region State Logic

    private void UpdateWindUp()
    {
        anim.SetAttackPhase(AttackAnimPhase.WindUp);
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        currentState = State.Dashing;
        anim.SetAttackPhase(AttackAnimPhase.Attack);
        timer = dashDuration;
        rb.linearVelocity = dashDirection * dashSpeed;
    }

    private void UpdateDash()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            StartRetreat();
        }
    }

    private void StartRetreat()
    {
        currentState = State.Retreating;
        timer = retreatDuration;

        retreatDirection = (rb.position - (Vector2)target.position).normalized;
        rb.linearVelocity = retreatDirection * retreatSpeed;
    }

    private void UpdateRetreat()
    {
        timer -= Time.deltaTime;

        float distanceToTarget = Vector2.Distance(rb.position, target.position);

        if (distanceToTarget >= retreatDistance || timer <= 0f)
        {
            StopAttack();
        }
    }
    private void UpdateCooldown()
    {
        if (Time.time >= lastAttackTime + stats.attackCooldown)
        {
            currentState = State.Idle;
        }
    }

    #endregion

    #region Collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState != State.Dashing) return;

        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            collision.gameObject
                .GetComponent<IDamageable>()
                ?.TakeDamage(stats.damage);

            StartRetreat();
        }
        else
        {
            // va tường / vật cản
            StartRetreat();
        }
    }

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
