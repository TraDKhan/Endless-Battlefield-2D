using System;
using UnityEngine;
public class EnemyBase : MonoBehaviour, IPoolable, IKnockbackable
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
    public event Action OnDeath;

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
        health.Init(stats.maxHealth);        
        OnSpawned();
    }

    public virtual void OnDespawn()
    {
        isAlive = false;        
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
        OnDeath?.Invoke();

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
    protected virtual void OnSpawned() { }    // effect / sound spawn
    protected virtual void ClearRuntimeEvents() { }
    #endregion
}
