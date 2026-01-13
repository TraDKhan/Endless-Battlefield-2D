using System.Collections.Generic;
using UnityEngine;

public class ObjectPool: IObjectPool
{
    private readonly Queue<GameObject> pool = new();
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly string poolKey;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        var identity = prefab.GetComponent<PoolIdentity>();
        if (identity == null)
        {
            Debug.LogError($"[Pool] Prefab {prefab.name} thiếu PoolIdentity");
            return;
        }

        poolKey = identity.PoolKey;

        for (int i = 0; i < initialSize; i++)
        {
            var go = CreateInstance();
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    private GameObject CreateInstance()
    {
        GameObject go = Object.Instantiate(prefab, parent);

        var identity = go.GetComponent<PoolIdentity>();
        if (identity == null)
            identity = go.AddComponent<PoolIdentity>();

        // clone dùng chung key
        typeof(PoolIdentity)
            .GetField("poolKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(identity, poolKey);

        return go;
    }

    public GameObject Spawn()
    {
        GameObject go = pool.Count > 0
            ? pool.Dequeue()
            : CreateInstance();

        go.SetActive(true);

        foreach (var poolable in go.GetComponentsInChildren<IPoolable>())
            poolable.OnSpawn();

        return go;
    }

    public void Despawn(GameObject obj)
    {
        if (!obj.activeSelf) return;

        foreach (var poolable in obj.GetComponentsInChildren<IPoolable>())
            poolable.OnDespawn();

        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
