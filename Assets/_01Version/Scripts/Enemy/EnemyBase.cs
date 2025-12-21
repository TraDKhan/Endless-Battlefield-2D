using System;
using UnityEngine;
public abstract class EnemyBase : MonoBehaviour, IPoolable
{
    [Header("Pool")]
    [SerializeField] public PoolIdentity Identity { get; set; }
    //public PoolIdentity Identity => identity;

    [Header("Base Stats")]
    [SerializeField] protected int maxHealth = 10;
    [SerializeField] protected float moveSpeed = 2f;

    protected EnemyHealthController health;
    protected Transform target;
    protected bool isAlive;

    public event Action<EnemyBase> OnEnemyDead;

    #region Unity
    protected virtual void Awake()
    {
        health = GetComponent<EnemyHealthController>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        health.Init(maxHealth);
        health.OnDeath += HandleDeath; // 🔥 chỉ subscribe 1 lần
    }
    #endregion

    #region Poolable
    public virtual void OnSpawn()
    {
        isAlive = true;

        health.ResetHP();
        health.Enable();

        ResetState();
        OnSpawned();
    }

    public virtual void OnDespawn()
    {
        isAlive = false;

        StopAllCoroutines();
        health.Disable();
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

    #region Virtual Hooks
    protected abstract void ResetState();     // reset AI / FSM / timers
    protected virtual void OnSpawned() { }    // effect / sound spawn
    protected virtual void ClearRuntimeEvents() { }
    #endregion
}
