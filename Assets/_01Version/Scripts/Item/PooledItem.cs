using UnityEngine;

[RequireComponent(typeof(PoolIdentity))]
public abstract class PooledItem : MonoBehaviour, IPoolable
{
    [Header("Pool")]
    public PoolIdentity Identity { get; set; }

    protected Transform player;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public virtual void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnDespawn()
    {
        gameObject.SetActive(false);
    }

    protected void DespawnSelf()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }
}
