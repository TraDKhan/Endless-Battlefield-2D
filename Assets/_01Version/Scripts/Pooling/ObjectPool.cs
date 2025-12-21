using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IObjectPool where T : Component
{
    private readonly Queue<T> pool = new();
    private readonly T prefab;
    private readonly Transform parent;

    private readonly PoolIdentity prefabIdentity;

    public ObjectPool(T prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        prefabIdentity = prefab.GetComponent<PoolIdentity>();
        if (prefabIdentity == null)
        {
            Debug.LogError($"[ObjectPool] Prefab {prefab.name} thiếu PoolIdentity");
            return;
        }

        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateInstance();
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    private T CreateInstance()
    {
        T obj = Object.Instantiate(prefab, parent);

        // ⭐ GÁN IDENTITY PREFAB GỐC
        if (obj is IPoolable poolable)
        {
            poolable.Identity = prefabIdentity;
        }

        return obj;
    }

    public Component Spawn()
    {
        T obj = pool.Count > 0
            ? pool.Dequeue()
            : CreateInstance();

        obj.gameObject.SetActive(true);

        if (obj is IPoolable poolable)
            poolable.OnSpawn();

        return obj;
    }

    public void Despawn(Component obj)
    {
        if (obj is not T t) return;

        // chống despawn 2 lần
        if (!t.gameObject.activeSelf) return;

        if (t is IPoolable poolable)
            poolable.OnDespawn();

        t.gameObject.SetActive(false);
        pool.Enqueue(t);
    }
}
