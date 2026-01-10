using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [Serializable]
    public class PoolConfig
    {
        [Tooltip("Prefab gốc (KHÔNG phải clone)")]
        public GameObject prefab;

        [Tooltip("Component chính để pool (EnemyBase, SpawnIndicator, Bullet...)")]
        public MonoBehaviour poolComponent;

        [Min(1)]
        public int initialSize = 10;
    }

    [Header("Pool Configs")]
    [SerializeField] private PoolConfig[] pools;

    /// <summary>
    /// Key = PoolIdentity (prefab gốc)
    /// Value = pool tương ứng
    /// </summary>
    private readonly Dictionary<PoolIdentity, IObjectPool> poolDict = new();

    #region Unity
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        InitializePools();
    }
    #endregion

    #region Init
    private void InitializePools()
    {
        foreach (var config in pools)
        {
            CreatePool(config);
        }
    }

    private void CreatePool(PoolConfig config)
    {
        if (config.prefab == null || config.poolComponent == null)
        {
            Debug.LogError("[PoolManager] PoolConfig thiếu prefab hoặc component");
            return;
        }

        PoolIdentity identity = config.prefab.GetComponent<PoolIdentity>();
        if (identity == null)
        {
            Debug.LogError(
                $"[PoolManager] Prefab {config.prefab.name} thiếu PoolIdentity"
            );
            return;
        }

        if (poolDict.ContainsKey(identity))
        {
            Debug.LogWarning(
                $"[PoolManager] Pool cho {config.prefab.name} đã tồn tại"
            );
            return;
        }

        Type componentType = config.poolComponent.GetType();
        Type poolType = typeof(ObjectPool<>).MakeGenericType(componentType);

        IObjectPool pool = (IObjectPool)Activator.CreateInstance(
            poolType,
            config.poolComponent,
            config.initialSize,
            transform
        );

        poolDict.Add(identity, pool);
    }
    #endregion

    #region Spawn
    public T Spawn<T>(GameObject prefab) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError("[PoolManager] Spawn prefab null");
            return null;
        }

        PoolIdentity identity = prefab.GetComponent<PoolIdentity>();
        if (identity == null)
        {
            Debug.LogError(
                $"[PoolManager] Prefab {prefab.name} thiếu PoolIdentity"
            );
            return null;
        }

        if (!poolDict.TryGetValue(identity, out var pool))
        {
            Debug.LogError(
                $"[PoolManager] Không tìm thấy pool cho {prefab.name}"
            );
            return null;
        }

        var obj = pool.Spawn();
        Debug.Assert(obj is T,
            $"Pool trả về {obj.GetType().Name} nhưng yêu cầu {typeof(T).Name}");

        return obj as T;
    }
    #endregion

    #region Despawn
    public void Despawn(IPoolable poolable)
    {
        if (poolable == null)
            return;

        PoolIdentity identity = poolable.Identity;
        if (identity == null)
        {
            Debug.LogError(
                $"[PoolManager] Poolable {poolable} thiếu PoolIdentity"
            );
            return;
        }

        if (!poolDict.TryGetValue(identity, out var pool))
        {
            Debug.LogError(
                $"[PoolManager] Không tìm thấy pool cho {identity.name}"
            );
            return;
        }

        pool.Despawn((Component)poolable);
    }
    #endregion
}