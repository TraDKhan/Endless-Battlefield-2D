using UnityEngine;

public interface IObjectPool
{
    GameObject Spawn();
    void Despawn(GameObject obj);
}
