using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [System.Serializable]
    public class PoolConfig
    {
        public Component prefab;
        public int initialSize = 10;
    }

    [SerializeField] private PoolConfig[] pools;

    private Dictionary<Component, object> poolDict = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (PoolConfig config in pools)
        {
            CreatePool(config);
        }
    }

    void CreatePool(PoolConfig config)
    {
        var poolType = typeof(ObjectPool<>).MakeGenericType(config.prefab.GetType());
        var pool = System.Activator.CreateInstance(
            poolType,
            config.prefab,
            config.initialSize,
            transform
        );

        poolDict.Add(config.prefab, pool);
    }

    public T Spawn<T>(T prefab) where T : Component
    {
        if (!poolDict.TryGetValue(prefab, out object poolObj))
        {
            Debug.LogError($"No pool for prefab {prefab.name}");
            return null;
        }

        return ((ObjectPool<T>)poolObj).Spawn();
    }

    public void Despawn<T>(T prefab, T instance) where T : Component
    {
        if (!poolDict.TryGetValue(prefab, out object poolObj))
            return;

        ((ObjectPool<T>)poolObj).Despawn(instance);
    }
}
