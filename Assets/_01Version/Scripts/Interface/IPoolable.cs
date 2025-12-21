using UnityEngine;

public interface IPoolable
{
    PoolIdentity Identity { get; set; }
    void OnSpawn();
    void OnDespawn();
}
