using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [Serializable]
    public class PoolConfig
    {
        public GameObject prefab;
        [Min(1)] public int initialSize = 10;
    }

    [SerializeField] private PoolConfig[] pools;

    private readonly Dictionary<string, IObjectPool> poolDict = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var config in pools)
            CreatePool(config);
    }

    private void CreatePool(PoolConfig config)
    {
        if (config.prefab == null) return;

        var identity = config.prefab.GetComponent<PoolIdentity>();
        if (identity == null)
        {
            Debug.LogError($"Prefab {config.prefab.name} thiếu PoolIdentity");
            return;
        }

        if (poolDict.ContainsKey(identity.PoolKey))
            return;

        var pool = new ObjectPool(
            config.prefab,
            config.initialSize,
            transform
        );

        poolDict.Add(identity.PoolKey, pool);
    }

    // ================= SPAWN =================
    public T Spawn<T>(GameObject prefab) where T : Component
    {
        var identity = prefab.GetComponent<PoolIdentity>();
        if (identity == null) return null;

        if (!poolDict.TryGetValue(identity.PoolKey, out var pool))
            return null;

        GameObject go = pool.Spawn();
        return go.GetComponent<T>();
    }

    // ================= DESPAWN =================
    public void Despawn(GameObject go)
    {
        var identity = go.GetComponent<PoolIdentity>();
        if (identity == null) return;

        if (!poolDict.TryGetValue(identity.PoolKey, out var pool))
            return;

        pool.Despawn(go);
    }

    public void Despawn(IPoolable poolable)
    {
        if (poolable == null) return;
        Despawn(((Component)poolable).gameObject);
    }
}