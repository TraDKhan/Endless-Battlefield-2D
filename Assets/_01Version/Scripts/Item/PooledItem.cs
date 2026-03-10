using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(PoolIdentity))]
public abstract class PooledItem : MonoBehaviour, IPoolable
{
    [Header("Pool")]
    public PoolIdentity Identity { get; set; }

    protected Transform player;
    [Header("Magnet")]
    public float magnetSpeed = 10f;

    private bool isMagnetActive;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public virtual void OnSpawn()
    {
        isMagnetActive = false;
        gameObject.SetActive(true);
    }

    public virtual void OnDespawn()
    {
        gameObject.SetActive(false);
    }

    public void StartMagnet(Transform target)
    {
        player = target;
        isMagnetActive = true;
    }

    protected virtual void Update()
    {
        if (!isMagnetActive || player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            magnetSpeed * Time.deltaTime
        );
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected();
            DespawnSelf();
        }
    }

    protected abstract void OnCollected();

    protected void DespawnSelf()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }
}