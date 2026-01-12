using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChargeAttack : MonoBehaviour, IEnemyAttack
{
    private EnemyContext ctx;
    private Rigidbody2D rb;

    [Header("Charge Settings")]
    [SerializeField] private float windUpTime = 0.4f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.6f;

    [Header("Retreat Settings")]
    [SerializeField] private float retreatDistance = 2.5f;
    [SerializeField] private float retreatSpeed = 4f;
    [SerializeField] private float retreatDuration = 0.6f;

    private float lastAttackTime;
    private float timer;

    private Vector2 dashDirection;
    private Vector2 retreatDirection;

    private enum State
    {
        Idle,
        WindUp,
        Dashing,
        Retreating,
        Cooldown
    }
    private State state = State.Idle;

    #region Init
    public void Init(EnemyContext context)
    {
        ctx = context;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    #endregion

    #region IEnemyAttack

    public bool CanAttack =>
        state == State.Idle &&
        Time.time >= lastAttackTime + ctx.Stats.attackCooldown;

    public float Cooldown => ctx.Stats.attackCooldown;

    public void StartAttack()
    {
        if (ctx.Target == null) return;

        state = State.WindUp;
        timer = windUpTime;

        dashDirection = (ctx.Target.position - transform.position).normalized;

        rb.linearVelocity = Vector2.zero;
        ctx.Anim?.PlayAttack(); // hoặc PlayChargeWindUp
    }

    public void UpdateAttack()
    {
        switch (state)
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
        lastAttackTime = Time.time;
        state = State.Cooldown;
    }

    #endregion

    #region State Logic

    void UpdateWindUp()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
            StartDash();
    }

    void StartDash()
    {
        state = State.Dashing;
        timer = dashDuration;
        rb.linearVelocity = dashDirection * dashSpeed;
    }

    void UpdateDash()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
            StartRetreat();
    }

    void StartRetreat()
    {
        state = State.Retreating;
        timer = retreatDuration;

        retreatDirection =
            ((Vector2)transform.position - (Vector2)ctx.Target.position).normalized;

        rb.linearVelocity = retreatDirection * retreatSpeed;
    }

    void UpdateRetreat()
    {
        timer -= Time.deltaTime;

        float dist = Vector2.Distance(
            transform.position,
            ctx.Target.position
        );

        if (dist >= retreatDistance || timer <= 0f)
            StopAttack();
    }

    void UpdateCooldown()
    {
        if (Time.time >= lastAttackTime + ctx.Stats.attackCooldown)
            state = State.Idle;
    }

    #endregion

    #region Collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state != State.Dashing) return;

        if (((1 << collision.gameObject.layer) & ctx.TargetLayer) != 0)
        {
            collision.gameObject
                .GetComponent<IDamageable>()
                ?.TakeDamage(ctx.Stats.damage);
        }

        StartRetreat();
    }

    #endregion
}
