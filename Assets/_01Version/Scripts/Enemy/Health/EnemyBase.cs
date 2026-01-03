using System;
using UnityEngine;
public abstract class EnemyBase : MonoBehaviour, IPoolable, IKnockbackable
{
    [Header("Pool")]
    [SerializeField] public PoolIdentity Identity { get; set; }

    [Header("Base Stats")]
    [SerializeField] protected EnemyStats stats;
    [SerializeField] private Rigidbody2D rb;

    protected EnemyHealthController health;
    protected Transform target;
    protected bool isAlive;

    public event Action<EnemyBase> OnEnemyDead;

    #region Unity
    protected virtual void Awake()
    {
        health = GetComponent<EnemyHealthController>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;

        health.OnDeath += HandleDeath;
    }
    #endregion

    #region Poolable
    public virtual void OnSpawn()
    {
        isAlive = true;

        health.ResetHealth();

        ResetState();
        OnSpawned();
    }

    public virtual void OnDespawn()
    {
        isAlive = false;
        health.Init(stats.maxHealth);
        StopAllCoroutines();
        ClearRuntimeEvents();
    }
    #endregion

    #region Death
    protected virtual void HandleDeath()
    {
        if (!isAlive) return;

        isAlive = false;
        OnEnemyDead?.Invoke(this);

        // 🔥 TRẢ VỀ POOL NGAY TẠI ĐÂY
        ObjectPoolManager.Instance.Despawn(this);
    }
    #endregion
    #region Knockback
    public void Knockback(Vector2 direction, float force)
    {
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }
    #endregion
    #region Virtual Hooks
    protected abstract void ResetState();     // reset AI / FSM / timers
    protected virtual void OnSpawned() { }    // effect / sound spawn
    protected virtual void ClearRuntimeEvents() { }
    #endregion
}
